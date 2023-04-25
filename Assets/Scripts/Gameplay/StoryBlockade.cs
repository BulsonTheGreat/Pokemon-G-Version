using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBlockade : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] Dialogs dialog;
    [SerializeField] Vector2 moveBack;
    [SerializeField] ItemSlot requiredItem;

    public void OnPlayerTriggered(PlayerMovement player)
    {
        var inventory = player.GetComponent<Inventory>().Items;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].Base.Name == requiredItem.Base.Name)
            {
                gameObject.SetActive(false);
            }
        }
        player.Character.Animator.IsMoving = false;
        StartCoroutine(DialogueManager.Instance.ShowDialogue(dialog));
        StartCoroutine(player.Character.Move(moveBack));
    }
}
