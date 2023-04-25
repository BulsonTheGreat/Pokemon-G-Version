using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class RandomizeMachine : MonoBehaviour, Interactable
{
    [SerializeField] List<Pokemon> availableMons;

    public IEnumerator Interact(Transform initiator)
    {
        if (availableMons.Count < 6)
        {
            yield return DialogueManager.Instance.ShowDialogueText("You've already used all of your options");
        }
        else
        {
            var playerParty = initiator.GetComponent<PokemonParty>().Pokemons;
            if (playerParty.Count > 0)
            {
                for (int i = (playerParty.Count - 1); i >= 0; i--)
                {
                    playerParty.Remove(playerParty[i]);
                }
            }
            //Randomize();
            for (int i = 0; i < 6; i++)
            {
                int r = Random.Range(0, availableMons.Count);
                playerParty.Add(availableMons[r]);
                availableMons[r].Init();
                availableMons.Remove(availableMons[r]);
            }
            Debug.Log($"There are {availableMons.Count} left to choose from");
            yield return DialogueManager.Instance.ShowDialogueText("Your party has been randomly selected");
        }
    }
}
