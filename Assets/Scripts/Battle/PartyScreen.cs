using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;
    [SerializeField] DetailsMenu detailedMenu;
    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;
    Pokemon pokemon;
    int selection;

    public Pokemon SelectedMember => pokemons[selection];

    public enum GameState {Partyscreem, Memberswitch }
    public BattleState? StateBeforeSwitch { get; set; }

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Pokemon> pokemons)
    {
        this.pokemons = pokemons;
        for(int i = 0; i < memberSlots.Length; i++)
        {
            if(i < pokemons.Count)
            {
                memberSlots[i].SetData(pokemons[i]);
            }
            else
            {
                memberSlots[i].gameObject.SetActive(false);
            }
        }
        UpdateMemberSelection(selection);
        detailedMenu.gameObject.SetActive(true);
        messageText.text = "Choose a pokemon";
    }

    public void HandleUpdate(Action OnSelected, Action OnBack)
    {
        var prevSelection = selection;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selection++;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selection--;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selection += 2;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selection -= 2;
        }
        selection = Mathf.Clamp(selection, 0, pokemons.Count - 1);

        if(selection != prevSelection)
            UpdateMemberSelection(selection);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            OnSelected?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            OnBack?.Invoke();
        }
    }


    public void UpdateMemberSelection(int selectedMember)
    {
        for(int i = 0; i < pokemons.Count; i++)
        {
            if(i == selectedMember)
            {
                memberSlots[i].SetSelected(true);
                detailedMenu.SetData(pokemons[selectedMember]);
            }
            else
            {
                memberSlots[i].SetSelected(false);
            }
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
