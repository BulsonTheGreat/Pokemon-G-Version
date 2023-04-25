using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialog, Cutscene, Menu, PartyScreen, DetailsTab, Paused }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] PartyScreen partyScreen;
    MenuController menuController;
    GameState state;
    GameState prevState;

    public bool QuestIsActive { get; set; } = false;

    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PrevScene { get; private set; }

    public static GameController Instance { get; private set; }

    //gain access to components and initiate variables
    public void Awake()
    {
        Instance = this;
        menuController = GetComponent<MenuController>();
        ConditionsDB.Init();
        PokemonDB.Init();
    }
    private void Start()
    {
        //subscribe to various events happening throughout the game and generate the right response
        battleSystem.OnTrainerBattleOver += EndBattle;

        DialogueManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        DialogueManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
            }
        };

        menuController.OnBack += () =>
        {
            state = GameState.FreeRoam;
        };

        menuController.OnMenuSelected += OnMenuSelected;

        partyScreen.Init();
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            prevState = state;
            state = GameState.Paused;
        }
        else
        {
            state = prevState;
        }
    }

    public void StartCutscene()
    {
        state = GameState.Cutscene;
    }

    public void StartFreeroam()
    {
        state = GameState.FreeRoam;
    }

    TrainerController trainer;
    public void StartBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        this.trainer = trainer;
        var playerParty = playerMovement.GetComponent<PokemonParty>();
        var trainerParty = trainer.GetComponent<PokemonParty>();
        battleSystem.StartBattle(playerParty, trainerParty);
    }
    public void OnEnterTrainersView(TrainerController trainer)
    {
        state = GameState.Cutscene;
        StartCoroutine(trainer.TriggerTrainerBattle(playerMovement));
    }

    void EndBattle(bool won)
    {
        if(won == true)
        {
            if (trainer.BattleMusic != null)
            {
                AudioManager.A.PlayMusic(CurrentScene.SceneSoundtrack, fade: true);
            }
            trainer.BattleLost();
            trainer = null;
        }
        else if(won == false)
        {
            if (trainer.BattleMusic != null)
            {
                AudioManager.A.PlayMusic(CurrentScene.SceneSoundtrack, fade: true);
            }
            StartCoroutine(RespawnPoint.Instance.RestartPosition(playerMovement));
        }
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }
    //depending on what state we're in grant access to different elements of the game
    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerMovement.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogueManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if (state == GameState.PartyScreen)
        {
            Action OnSelected = () =>
            {
                //Try to switch pokemon? 
            };
            Action OnBack = () =>
            {
                partyScreen.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };
            partyScreen.HandleUpdate(OnSelected, OnBack);
        }
    }

    void OnMenuSelected(int selectedItem)
    {
        if(selectedItem == 0)
        {
            //pokemon party screen is selected
            var pokemons = playerMovement.GetComponent<PokemonParty>().Pokemons;
            if(pokemons.Count == 0)
            {
                StartCoroutine(DialogueManager.Instance.ShowDialogueText($"You don't have any pokemon yet, go ahead and grab them"));
            }
            else
            {
                partyScreen.gameObject.SetActive(true);
                partyScreen.SetPartyData(playerMovement.GetComponent<PokemonParty>().Pokemons);
                state = GameState.PartyScreen;
            }
        }
        else if(selectedItem == 1)
        {
            if(!QuestIsActive)
            {
                //save
                SavingSystem.i.Save("saveSlot1");
                StartCoroutine(DialogueManager.Instance.ShowDialogueText($"{playerMovement.Name} saved the game"));
                state = GameState.FreeRoam;
            }
            else
            {
                StartCoroutine(DialogueManager.Instance.ShowDialogueText($"You can't save the game while a quest is active"));
                state = GameState.FreeRoam;
            }
        }
        else if(selectedItem == 2)
        {
            if (!QuestIsActive)
            {
                //load
                SavingSystem.i.Load("saveSlot1");
                StartCoroutine(DialogueManager.Instance.ShowDialogueText($"Loaded the most recent save file"));
                state = GameState.FreeRoam;
            }
            else
            {
                StartCoroutine(DialogueManager.Instance.ShowDialogueText($"You can't load the game while a quest is active"));
                state = GameState.FreeRoam;
            }
        }
        else if(selectedItem == 3)
        {
            //exit the game
            Application.Quit();
        }
    }
    public void SetCurrentScene(SceneDetails currScene)
    {
        PrevScene = CurrentScene;
        CurrentScene = currScene;
    }
}
