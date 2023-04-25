using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour, ISavable
{
    [SerializeField] ItemSlot item;
    [SerializeField] Dialogs dialog;

    bool ItemGiven = false;

    public IEnumerator GiveItem(PlayerMovement player)
    {
        yield return DialogueManager.Instance.ShowDialogue(dialog);

        var playerItems = player.GetComponent<Inventory>().Items;
        playerItems.Add(item);
        yield return DialogueManager.Instance.ShowDialogueText($"{player.Name} received {item.Base.name}");
        ItemGiven = true;
    }

    public bool CanBeGiven()
    {
        return item != null && !ItemGiven;
    }

    public object CaptureState()
    {
        return ItemGiven;
    }

    public void RestoreState(object state)
    {
        ItemGiven = (bool)state;
    }
}
