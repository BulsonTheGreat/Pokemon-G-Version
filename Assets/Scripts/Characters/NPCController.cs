using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialogs lines;
    [SerializeField] QuestBasics quest;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPatterns;
    Character character;
    Healer healer;
    ItemGiver giver;
    [SerializeField]
    NPCState state;
    float idleTimer = 0f;
    int currentMovementPattern = 0;
    Quest activeQuest;

    public void Awake()
    {
        character = GetComponent<Character>();
        healer = GetComponent<Healer>();
        giver = GetComponent<ItemGiver>();
    }

    public IEnumerator Interact(Transform initiator)
    {
        Inventory inventory = initiator.GetComponent<Inventory>();
        if(state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            character.LookTowards(initiator.position);
            if(healer != null)
            {
                yield return healer.Heal(initiator, lines);
                idleTimer = 0f;
                state = NPCState.Idle;
            }
            else if(giver != null && giver.CanBeGiven() == true)
            {
                yield return giver.GiveItem(initiator.GetComponent<PlayerMovement>());
                idleTimer = 0f;
                state = NPCState.Idle;
            }
            else if(quest != null)
            {
                activeQuest = new Quest(quest, inventory);
                yield return activeQuest.StartQuest();
                GameController.Instance.QuestIsActive = true;
                state = NPCState.Idle;
                quest = null;
            }
            else if(activeQuest != null)
            {
                if(activeQuest.CanBeCompleted(initiator))
                {
                    yield return activeQuest.EndQuest(initiator);
                    GameController.Instance.QuestIsActive = false;
                    state = NPCState.Idle;
                    activeQuest = null;
                }
                else
                {
                    yield return DialogueManager.Instance.ShowDialogue(activeQuest.Basics.InProgressDialog);
                    state = NPCState.Idle;
                }
            }
            else
            {
                yield return DialogueManager.Instance.ShowDialogue(lines);
                idleTimer = 0f;
                state = NPCState.Idle;
            }
        }
    }

    private void Update()
    {
        if(state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if(idleTimer > timeBetweenPatterns)
            {
                idleTimer = 0f;
                if(movementPattern.Count > 0)
                {
                    StartCoroutine(Walk());
                }
            }
        }
        character.HandleUpdate();
    }

    IEnumerator Walk()
    {
        state = NPCState.Walking;

        var oldPos = transform.position;

        yield return character.Move(movementPattern[currentMovementPattern]);
        if (transform.position != oldPos)
        {
            currentMovementPattern = (currentMovementPattern + 1) % movementPattern.Count;
        }

        state = NPCState.Idle;
    }

}

public enum NPCState { Idle, Walking, Dialog}
