using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Quest
{
    public QuestBasics Basics { get; private set; }
    public QuestStatus Status { get; private set; }
    public Inventory Inventory { get; private set; }

    public Quest(QuestBasics basics, Inventory inventory)
    {
        Basics = basics;
        Inventory = inventory;
    }

    public IEnumerator StartQuest()
    {
        Status = QuestStatus.started;
        GameController.Instance.QuestIsActive = true;
        yield return DialogueManager.Instance.ShowDialogue(Basics.StartDialog);
    }

    public IEnumerator EndQuest(Transform player)
    {
        Status = QuestStatus.completed;
        GameController.Instance.QuestIsActive = false;
        yield return DialogueManager.Instance.ShowDialogue(Basics.EndDialog);

        var inventory = player.GetComponent<Inventory>().Items;
        if(Basics.RequiredItem != null)
        {
            inventory.Remove(Basics.RequiredItem);
        }

        if(Basics.RewardItem != null)
        {
            string playerName = player.GetComponent<PlayerMovement>().Name;

            inventory.Add(Basics.RewardItem);
            yield return DialogueManager.Instance.ShowDialogueText($"{playerName} received {Basics.RewardItem.Base.Name}");
        }
    }

    public bool CanBeCompleted(Transform player)
    {
        if(Basics.RequiredItem != null)
        {
            var inventory = player.GetComponent<Inventory>().Items;
            for(int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].Base.Name == Basics.RequiredItem.Base.Name)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

public enum QuestStatus { none, started, completed}
