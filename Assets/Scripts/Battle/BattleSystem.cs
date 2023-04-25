using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Threading.Tasks;

public enum BattleState {Start, ActionSelection, MoveSelection, RunningTurn, Busy, PartyScreen, FillerAction, EndingStatement, BattleOver }
public enum BattleAction { Move, SwitchPokemon, UseItem, Run}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;

    [SerializeField] Image playerImage;
    [SerializeField] Image enemyImage;

    public event Action<bool> OnTrainerBattleOver;
    
    BattleState state;
    int currentAction;
    int currentMove;
    private bool playedAnim = true;

    PokemonParty playerParty;
    PokemonParty enemyParty;

    PlayerMovement player;
    TrainerController trainer;

    public void StartBattle(PokemonParty playerParty, PokemonParty enemyParty)
    {
        this.playerParty = playerParty;
        this.enemyParty = enemyParty;

        player = playerParty.GetComponent<PlayerMovement>();
        trainer = enemyParty.GetComponent<TrainerController>();

        if (trainer.BattleMusic != null)
        {
            AudioManager.A.PlayMusic(trainer.BattleMusic);
        }

        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Hud.gameObject.SetActive(false);
        enemyUnit.Hud.gameObject.SetActive(false);
        //Disable units
        playerUnit.gameObject.SetActive(false);
        enemyUnit.gameObject.SetActive(false);
        //Show trainers
        playerImage.gameObject.SetActive(true);
        enemyImage.gameObject.SetActive(true);
        playerImage.sprite = player.Sprite;
        enemyImage.sprite = trainer.Sprite;
        yield return dialogBox.TypeDialog($"{trainer.Class} {trainer.Name} wants to battle");

        partyScreen.Init();

        //send trainer pokemon
        enemyImage.gameObject.SetActive(false);
        enemyUnit.gameObject.SetActive(true);
        var enemyPokemon = enemyParty.GetHealthyPokemon();
        enemyUnit.Setup(enemyPokemon);
        yield return dialogBox.TypeDialog($"{trainer.Class} {trainer.Name} sends out {enemyPokemon.Base.Name}");
        //send player pokemon
        playerImage.gameObject.SetActive(false);
        playerUnit.gameObject.SetActive(true);
        var playerPokemon = playerParty.GetHealthyPokemon();
        playerUnit.Setup(playerPokemon);
        yield return dialogBox.TypeDialog($"Go {playerPokemon.Base.Name}");
        //set up moves based on the pokemon
        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
        ActionSelection();
    }

    void BattleOver(bool won)
    {
        //clear all the stat boosts and status conditions after battle ends
        state = BattleState.BattleOver;
        playerParty.Pokemons.ForEach(p => p.OnBattleOver());
        enemyParty.Pokemons.ForEach(p => p.OnBattleOver());
        OnTrainerBattleOver(won);
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        partyScreen.StateBeforeSwitch = state;
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    void RunAway()
    {
        state = BattleState.FillerAction;
        dialogBox.EnableActionSelector(false);
        StartCoroutine(dialogBox.TypeDialog("You can't run away from a trainer battle"));
        Invoke(nameof(ActionSelection), 2f);
    }

    void OpenBag()
    {
        state = BattleState.FillerAction;
        dialogBox.EnableActionSelector(false);
        StartCoroutine(dialogBox.TypeDialog("You're too broke to buy items"));
        Invoke(nameof(ActionSelection), 2f);
    }

    IEnumerator RunTurns(BattleAction playerAction)
    {
        state = BattleState.RunningTurn;
        if(playerAction == BattleAction.Move)
        {
            playerUnit.Pokemon.CurrentMove = playerUnit.Pokemon.Moves[currentMove];
            enemyUnit.Pokemon.CurrentMove = enemyUnit.Pokemon.GetRandomMove();
            bool playerFirst;
            //checking for move priorities
            if(enemyUnit.Pokemon.CurrentMove.Base.Priority == true && playerUnit.Pokemon.CurrentMove.Base.Priority == false)
            {
                playerFirst = false;
            }
            else if(playerUnit.Pokemon.CurrentMove.Base.Priority == true && enemyUnit.Pokemon.CurrentMove.Base.Priority == false)
            {
                playerFirst= true;
            }
            else
            {
                playerFirst = playerUnit.Pokemon.Speed >= enemyUnit.Pokemon.Speed;
            }
            //check who goes first
            var firstUnit = (playerFirst) ? playerUnit : enemyUnit;
            var secondUnit = (playerFirst) ? enemyUnit : playerUnit;

            var secondPokemon = secondUnit.Pokemon;
            //first turn
            yield return RunMove(firstUnit, secondUnit, firstUnit.Pokemon.CurrentMove);
            if (state == BattleState.BattleOver)
            {
                yield break;
            }
            //if the second unit is still alive it can execute it's move
            if(secondPokemon.HP > 0)
            {
                //second turn
                yield return RunMove(secondUnit, firstUnit, secondUnit.Pokemon.CurrentMove);
                if (state == BattleState.BattleOver)
                {
                    yield break;
                }
            }
            //poison/burn damage for first unit
            yield return RunAfterTurn(firstUnit);
            //poison/burn damage for second unit
            if (secondPokemon.HP > 0)
            {
                //second turn
                yield return RunAfterTurn(secondUnit);
                if (state == BattleState.BattleOver)
                {
                    yield break;
                }
            }

        }
        else
        {
            //if you choose to switch
            if(playerAction == BattleAction.SwitchPokemon)
            {
                var selectedMember = partyScreen.SelectedMember;
                state = BattleState.Busy;
                yield return SwitchPokemon(selectedMember);
            }
            //then enemy gets to attack
            var enemyMove = enemyUnit.Pokemon.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove);
            yield return RunAfterTurn(enemyUnit);
            if (state == BattleState.BattleOver) yield break;
        }
        if(state != BattleState.BattleOver)
        {
            ActionSelection();
        }
    }
    
    //all the logic used while a move is cast
    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        bool canRunMove = sourceUnit.Pokemon.OnBeforeTurn();
        if(!canRunMove)
        {
            yield return ShowStatusChanges(sourceUnit.Pokemon, sourceUnit);
            yield return sourceUnit.Hud.UpdateHP();
            //just in case you die from confusion
            if (sourceUnit.Pokemon.HP <= 0)
            {
                yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} fainted");
                sourceUnit.PlayFaintAnimation();
                sourceUnit.Hud.gameObject.SetActive(false);
                
                yield return new WaitForSeconds(2f);
                CheckForBattleOver(sourceUnit);
            }
            yield break;
        }
        yield return ShowStatusChanges(sourceUnit.Pokemon, sourceUnit);

        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} used {move.Base.Name}");
        //checking if it connects
        if(CheckIfMoveHits(move, sourceUnit.Pokemon, targetUnit.Pokemon) == true)
        {
            sourceUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            //if a move is a status condition apply it's effects
            if (move.Base.MoveCategorie == MoveCategories.Status)
            {
                //FFS (yield return not StartCoroutine)!!!!!!!!!!!!!!!!
                yield return RunMoveEffects(move.Base.Effects, sourceUnit.Pokemon, targetUnit.Pokemon, sourceUnit, targetUnit, move.Base.Targets);
            }
            //if not then a pokemon should take damage
            else
            {
                targetUnit.PlayHitAnimation();
                var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon, targetUnit.Pokemon);
                yield return targetUnit.Hud.UpdateHP();
                yield return ShowDamageDetails(damageDetails);
                //if move also restores HP
                if (move.Base.RestoresHP && sourceUnit.Pokemon.HP < sourceUnit.Pokemon.MaxHP)
                {
                    yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} restored it's HP");
                    yield return sourceUnit.Hud.UpdateHP();
                }
                //apply recoil damage
                if (move.Base.HasRecoil)
                {
                    yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} is hurt by recoil");
                    yield return sourceUnit.Hud.UpdateHP();
                }
                //check for secondary effects
                if (move.Base.SecondaryEffects != null && move.Base.SecondaryEffects.Count > 0)
                {
                    if (targetUnit.Pokemon.HP > 0)
                    {
                        foreach (var secondary in move.Base.SecondaryEffects)
                        {
                            var rnd = UnityEngine.Random.Range(1, 100);
                            if (rnd <= secondary.Chance)
                            {
                                //in case applies a status condition to a user (basically outrage)
                                if(secondary.SelfTarget == true)
                                {
                                    yield return RunMoveEffects(secondary, sourceUnit.Pokemon, sourceUnit.Pokemon, sourceUnit, sourceUnit, secondary.Target);
                                }
                                //99% of the time
                                else
                                {
                                    yield return RunMoveEffects(secondary, sourceUnit.Pokemon, targetUnit.Pokemon, sourceUnit, targetUnit, secondary.Target);
                                }
                            }
                        }
                    }
                }
            }

            //if the hp drops below 0 a unit faints
            if (targetUnit.Pokemon.HP <= 0)
            {
                yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name} fainted");
                targetUnit.PlayFaintAnimation();
                targetUnit.Hud.gameObject.SetActive(false);
                //in case attacker's move affects them in some way
                foreach (var secondary in move.Base.SecondaryEffects)
                {
                    var rnd2 = UnityEngine.Random.Range(1, 100);
                    if (secondary.Chance >= rnd2 && secondary.Target == MoveTargets.Self)
                    {
                        yield return RunMoveEffects(secondary, sourceUnit.Pokemon, targetUnit.Pokemon, sourceUnit, targetUnit, secondary.Target);
                    }
                }

                yield return new WaitForSeconds(2f);
                CheckForBattleOver(targetUnit);
            }
            
        }
        else
        {
            yield return dialogBox.TypeDialog("But it missed!");
        }
    }
    IEnumerator RunAfterTurn(BattleUnit sourceUnit)
    {
        if(state == BattleState.BattleOver)
        {
            yield break;
        }
        yield return new WaitUntil(() => state == BattleState.RunningTurn);
        //for status conditions that hurt a unit (poison, burn) 
        sourceUnit.Pokemon.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.Pokemon, sourceUnit);
        yield return sourceUnit.Hud.UpdateHP();
        //if a unit faints from status condition
        if (sourceUnit.Pokemon.HP <= 0)
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} fainted");
            sourceUnit.PlayFaintAnimation();
            sourceUnit.Hud.gameObject.SetActive(false);

            yield return new WaitForSeconds(2f);
            CheckForBattleOver(sourceUnit);
            yield return new WaitUntil(() => state == BattleState.RunningTurn);
        }
    }
    
    //defining all the logic used for status conditions 
    IEnumerator RunMoveEffects(MoveEffects effects, Pokemon source, Pokemon target, BattleUnit sourceUnit, BattleUnit targetUnit, MoveTargets moveTarget)
    {
        //Boost or debuff statistics
        if(effects.Boosts != null)
        {
            if(moveTarget == MoveTargets.Self)
            {
                source.ApplyBoost(effects.Boosts);
                sourceUnit.PlayBoostAnimation();
            }
            else
            {
                target.ApplyBoost(effects.Boosts);
                targetUnit.PlayDebuffAnimation();
            }
        }
        
        //Apply status conditions
        if(effects.Status != ConditionID.none && target.Status == null)
        {
            target.SetStatus(effects.Status);
            targetUnit.SetStatusEffect(effects.Status);
            targetUnit.ShowStatusEffects(playedAnim);
            playedAnim = false;
        }
        //Or don't if it already exists
        else if (effects.Status != ConditionID.none && target.Status != null)
        {
            yield return dialogBox.TypeDialog($"{target.Base.Name} already has a status condition");
        }
        //Apply volatile status condition (confusion and flinch)
        if(effects.VolatileStatus != ConditionID.none && target.VolatileStatus == null)
        {
            target.SetVolatileStatus(effects.VolatileStatus);
            targetUnit.SetStatusEffect(effects.VolatileStatus);
            playedAnim = false;
        }
        //So that you can't permamently confuse someone
        else if(effects.VolatileStatus != ConditionID.none && target.VolatileStatus != null)
        {
            yield return dialogBox.TypeDialog($"{target.Base.Name} is already confused");
        }
        yield return ShowStatusChanges(source, sourceUnit);
        yield return ShowStatusChanges(target, targetUnit);
        playedAnim = true;
    }

    bool CheckIfMoveHits(Move move, Pokemon source, Pokemon target)
    {
        if(move.Base.AlwaysHits == true)
        {
            return true;
        }
        float moveAccuracy = move.Base.Accuracy;
        int accuracy = source.StatBoosts[Stat.Accuracy];

        var boostValues = new float[] {1f, 4f/3f, 5f/3f, 2f, 7f/3f, 8f/3f, 3f };
        if(accuracy > 0)
        {
            moveAccuracy *= boostValues[accuracy];
        }
        else
        {
            moveAccuracy /= boostValues[-accuracy];
        }
        return UnityEngine.Random.Range(1, 100) <= moveAccuracy;
    }
    //opponent sends out next pokemon if they still have any
    IEnumerator SendNextPokemon(Pokemon nextPokemon)
    {
        state = BattleState.Busy;
        enemyUnit.Hud.gameObject.SetActive(false);
        enemyUnit.Setup(nextPokemon);
        yield return dialogBox.TypeDialog($"{trainer.Class} {trainer.Name} sends out {nextPokemon.Base.Name}");
        state = BattleState.RunningTurn;
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if(faintedUnit.IsPlayerUnit == true)
        {
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
            {
                //if you still have healthy unit you can send them out
                OpenPartyScreen();
            }
            else
            {
                //otherwise you lost
                trainer.RestartUnits();
                StartCoroutine(YouLost());
            }
        }
        //if the fainted pokemon didn't belong to you than tell your opponent to send next one
        else
        {
            var nextUnit = enemyParty.GetHealthyPokemon();
            if (nextUnit != null)
            {
                //if there are any more healthy ones then send them out
                StartCoroutine(SendNextPokemon(nextUnit));
            }
            else
            {
                //you won
                StartCoroutine(PartingWords());
            }
        }
    }
    IEnumerator PartingWords()
    {
        state = BattleState.EndingStatement;
        StartCoroutine(dialogBox.TypeDialog($"{player.Name} defeated {trainer.Class} {trainer.Name}"));
        yield return new WaitForSeconds(3f);
        enemyImage.gameObject.SetActive(true);
        StartCoroutine(dialogBox.TypeDialog($"{trainer.FinalWords}"));
        yield return new WaitForSeconds(3f);
        state = BattleState.BattleOver;
        BattleOver(true);
    }
    IEnumerator YouLost()
    {
        state = BattleState.EndingStatement;
        yield return dialogBox.TypeDialog($"{trainer.Class} {trainer.Name} defeated {player.Name}");
        enemyUnit.gameObject.SetActive(false);
        enemyUnit.Hud.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        enemyImage.gameObject.SetActive(true);
        yield return dialogBox.TypeDialog($"{trainer.FinalWords}");
        yield return new WaitForSeconds(1.5f);
        yield return dialogBox.TypeDialog($"{player.Name} ran to the proffesor's lab as fast as he could");
        yield return new WaitForSeconds(1.5f);
        state = BattleState.BattleOver;
        BattleOver(false);
    }
    //display wheter a move was super, not very or not effective at all (or if you landed a crit
    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.TypeEffectiveness == 0f)
        {
            yield return dialogBox.TypeDialog($"It doesn't effect opposing Pokemon!");
        }
        else
        {
            if (damageDetails.Critical > 1f)
            {
                yield return dialogBox.TypeDialog("A critical hit!");
            }
            if (damageDetails.TypeEffectiveness > 1f)
            {
                yield return dialogBox.TypeDialog("It's super effective!");
            }
            else if (damageDetails.TypeEffectiveness < 1f && damageDetails.TypeEffectiveness > 0f)
            {
                yield return dialogBox.TypeDialog("It's not very effective!");
            }
        }
    }
    //display a message if status condition has been activated
    IEnumerator ShowStatusChanges(Pokemon pokemon, BattleUnit unit)
    {
        while(pokemon.StatusChanges.Count > 0)
        {
            if(pokemon.StatusChanges.Count > 0)
            {
                var message = pokemon.StatusChanges.Dequeue();
                yield return dialogBox.TypeDialog(message);
            }
            unit.ShowStatusEffects(playedAnim);
            yield return new WaitForSeconds(0.75f);
        }
    }

    public void HandleUpdate()
    {
        if(state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if(state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if(state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }
    //deciding which action should take effect based on your input
    void HandleActionSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentAction++;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentAction--;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentAction += 2;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentAction -= 2;
        }
        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(currentAction == 0)
            {
                //choose a move
                MoveSelection();
            }
            else if(currentAction == 1)
            {
                //Switch Pokemon
                OpenPartyScreen();
            }
            else if (currentAction == 2)
            {
                //Open bag
                OpenBag();
            }
            else if (currentAction == 3)
            {
                //Type a message
                RunAway();
            }
        }
    }
    //deciding which move is used based on your input
    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentMove++;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentMove--;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        { 
            currentMove+=2; 
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        { 
            currentMove-=2;
        }

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Pokemon.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(RunTurns(BattleAction.Move));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            ActionSelection();
        }
    }
    //deciding which party member to send out based on your input
    void HandlePartySelection()
    {
        Action OnSelected = () =>
        {
            var selectedMember = partyScreen.SelectedMember;
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't send out a fainted pokemon");
                return;
            }
            if (selectedMember == playerUnit.Pokemon)
            {
                partyScreen.SetMessageText($"{playerUnit.Pokemon.Base.Name} is already in battle");
                return;
            }
            partyScreen.gameObject.SetActive(false);
            //if player decided to switch during a turn 
            if (partyScreen.StateBeforeSwitch == BattleState.ActionSelection)
            {
                StartCoroutine(RunTurns(BattleAction.SwitchPokemon));
            }
            //if player sends out next unit after previous fainted
            else
            {
                state = BattleState.Busy;
                StartCoroutine(SwitchPokemon(selectedMember));
            }
            partyScreen.StateBeforeSwitch = null;
        };

        Action OnBack = () =>
        {
            //if your pokemon just fainted you must send out next one
            if(playerUnit.Pokemon.HP <= 0)
            {
                partyScreen.SetMessageText("You have to send out the next pokemon");
                return;
            }
            //else if you changed your mind
            partyScreen.gameObject.SetActive(false);
            ActionSelection();
            partyScreen.StateBeforeSwitch = null;
        };
        partyScreen.HandleUpdate(OnSelected, OnBack);

    }
    //all the logic used while a pokemon is switched
    IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        if(playerUnit.Pokemon.HP > 0)
        {
            dialogBox.EnableActionSelector(false);
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} return");
            playerUnit.Pokemon.ResetStatBoosts();
            playerUnit.Pokemon.CureVolatileStatus();
            playerUnit.Hud.gameObject.SetActive(false);
            playerUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name}");
        playerUnit.Setup(newPokemon);
        dialogBox.SetMoveNames(newPokemon.Moves);

        state = BattleState.RunningTurn;
    }
}
