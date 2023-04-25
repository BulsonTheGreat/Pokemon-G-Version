using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonBlockade : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] Dialogs dialog1;
    [SerializeField] Dialogs dialog2;
    [SerializeField] Vector2 moveBack;

    public void OnPlayerTriggered(PlayerMovement player)
    {
        var party = player.GetComponent<PokemonParty>().Pokemons;
        if (party.Count == 0)
        {
            player.Character.Animator.IsMoving = false;
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialog1));
            StartCoroutine(player.Character.Move(moveBack));
        }
        else
        {
            for(int i = 0; i < party.Count; i++)
            {
                if(party[i].HP == 0)
                {
                    player.Character.Animator.IsMoving = false;
                    StartCoroutine(DialogueManager.Instance.ShowDialogue(dialog2));
                    StartCoroutine(player.Character.Move(moveBack));
                    break;
                }
            }
        }
    }
}
