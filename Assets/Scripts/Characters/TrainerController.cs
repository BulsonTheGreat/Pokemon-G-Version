using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainerController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] string trainerClass;
    [SerializeField] string trainerName;
    [SerializeField] GameObject exclamationPoint;
    Character character;
    Healer healer;
    ItemGiver giver;
    [SerializeField] Dialogs dialog;
    [SerializeField] Dialogs lastStatement;
    [SerializeField] GameObject fov;
    [SerializeField] Sprite sprite;
    [SerializeField] bool BattleBoss = false;
    PokemonParty trainerParty;
    PlayerMovement player;
    Cutscene cutscene;

    [SerializeField] AudioClip battleMusic;
    //state
    bool battleLost = false;

    private void Awake()
    {
        character = GetComponent<Character>();
        healer = GetComponent<Healer>();
        trainerParty = GetComponent<PokemonParty>();
        giver = GetComponent<ItemGiver>();
        cutscene = GetComponent<Cutscene>();
    }
    //set fov direction at the very start if a trainer has it
    private void Start()
    {
        if(fov != null)
            SetFovDirection(character.Animator.DefaultDirection);
    }
    //what happens when you try to interact with the trainer
    public IEnumerator Interact(Transform initiator)
    {
        character.LookTowards(initiator.position);
        //if you haven't defeated them yet
        if (battleLost == false)
        {
            if(BattleBoss == false)
            {
                yield return DialogueManager.Instance.ShowDialogue(dialog);
                GameController.Instance.StartBattle(this);
            }
            else if(BattleBoss == true)
            {
                //If the're a boss then they only bother with you when the quest is active
                if(GameController.Instance.QuestIsActive == true)
                {
                    yield return DialogueManager.Instance.ShowDialogue(dialog);
                    GameController.Instance.StartBattle(this);
                }
                else
                {
                    yield return DialogueManager.Instance.ShowDialogue(LastStatement);
                }
            }

        }
        else
        {
            //if they can heal your units
            if(healer != null)
            {
                yield return healer.Heal(initiator, lastStatement);
            }
            //else they're just salty
            else
            {
                yield return DialogueManager.Instance.ShowDialogue(lastStatement);
            }
        }
    }
    //how to trigger someone by existing
    public IEnumerator TriggerTrainerBattle(PlayerMovement player)
    {
        this.player = player;
        //show the exclamation point (for most trainers)
        if(exclamationPoint != null)
        {
            exclamationPoint.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            exclamationPoint.SetActive(false);
        }
        player.Character.LookTowards(gameObject.transform.position);
        //start dialogue
        yield return DialogueManager.Instance.ShowDialogue(dialog);
        GameController.Instance.StartBattle(this);
    }
    //In case the trainer somehow wins next time you battle they should send out healthy pokemon
    public void RestartUnits()
    {
        trainerParty.Pokemons.ForEach(p => p.Heal());
    }

    public void BattleLost()
    {
        //What happens after they lose their battle
        battleLost = true;
        //Disable their field of view
        if(fov != null)
            fov.SetActive(false);
        //Give an item if they have any
        if (giver != null && giver.CanBeGiven() == true)
        {
            StartCoroutine(giver.GiveItem(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>()));
        }
        //Play a cutscene if one is assigned
        if (cutscene != null)
        {
            StartCoroutine(cutscene.Play());
        }
    }
    //Set the direction of their field of view
    public void SetFovDirection(FacingDir dir)
    {
        float angle = 0;
        if(dir == FacingDir.Right)
        {
            angle = 90f;
        }
        else if (dir == FacingDir.Up)
        {
            angle = 180f;
        }
        else if (dir == FacingDir.Left)
        {
            angle = 270f;
        }
        if(fov != null)
            fov.transform.eulerAngles = new Vector3(0, 0, angle);
        
    }

    public object CaptureState()
    {
        return battleLost;
    }

    public void RestoreState(object state)
    {
        battleLost = (bool)state;
        if(battleLost == false)
        {
            RestartUnits();
        }

        if(battleLost == true)
        {
            fov.SetActive(false);
        }
    }

    public string Class
    {
        get => trainerClass;
    }
    public string Name
    {
        get => trainerName;
    }
    public Dialogs LastStatement
    {
        get => lastStatement;
    }
    public string FinalWords
    {
        get { return lastStatement.Lines[0].ToString(); }
    }
    public bool LostBattle
    {
        get => battleLost;
    }
    public Sprite Sprite
    {
        get => sprite;
    }
    public AudioClip BattleMusic
    {
        get => battleMusic;
    }
}
