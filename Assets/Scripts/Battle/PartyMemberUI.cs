using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] HpBar hpBar;
    [SerializeField] Image pkmSprite;
    Pokemon _pokemon;
    
    public void SetData(Pokemon pokemon)
    {
        gameObject.SetActive(true);
        _pokemon = pokemon;
        nameText.text = pokemon.Base.Name;
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHP);
        pkmSprite.sprite = pokemon.Base.MenuSprite;
    }
    public void SetSelected(bool selected)
    {
        if(selected)
        {
            nameText.color = GlobalSettings.I.HighlightedColor;
        }
        else
        {
            DeSelect();
        }
    }

    public void SetPreselected()
    {
        nameText.color = Color.red;
    }
    public void DeSelect()
    {
        nameText.color = Color.black;
    }
}
