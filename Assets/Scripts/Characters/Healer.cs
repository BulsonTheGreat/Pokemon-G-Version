using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    Fader fader;
    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    public IEnumerator Heal(Transform player, Dialogs dialog)
    {
        yield return DialogueManager.Instance.ShowDialogue(dialog);
        yield return fader.FadeIn(1.5f);
        var playerParty = player.GetComponent<PokemonParty>();
        playerParty.Pokemons.ForEach(p => p.Heal());
        playerParty.PartyUpdated();
        yield return fader.FadeOut(0.5f);
    }
}
