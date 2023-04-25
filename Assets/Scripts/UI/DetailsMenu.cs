using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DetailsMenu : MonoBehaviour
{
    [SerializeField] Image pkmSprite;
    [SerializeField] Text nameText;
    [SerializeField] Text descriptionText;
    [SerializeField] Image type1Image;
    [SerializeField] Image type2Image;
    [SerializeField] Text hpText;
    [SerializeField] Text attackText;
    [SerializeField] Text defenseText;
    [SerializeField] Text spattackText;
    [SerializeField] Text spdefenseText;
    [SerializeField] Text speedText;
    [SerializeField] Text move1Text;
    [SerializeField] Text move2Text;
    [SerializeField] Text move3Text;
    [SerializeField] Text move4Text;

    
    public void SetData(Pokemon pokemon)
    {
        pkmSprite.sprite = pokemon.Base.FrontSprite;
        nameText.text = pokemon.Base.Name;
        descriptionText.text = pokemon.Base.Description;

        type1Image.sprite = GlobalSettings.I.TypeImages[pokemon.Base.Type1];
        type2Image.sprite = GlobalSettings.I.TypeImages[pokemon.Base.Type2];

        hpText.text = $"HP: {pokemon.HP}/{pokemon.MaxHP}";
        attackText.text = $"Attack: {pokemon.Base.Attack}";
        defenseText.text = $"Defense: {pokemon.Base.Defense}";
        spattackText.text = $"SpAttack: {pokemon.Base.SpAttack}";
        spdefenseText.text = $"SpDefense: {pokemon.Base.SpDefense}";
        speedText.text = $"Speed: {pokemon.Base.Speed}";

        move1Text.text = $"{pokemon.Base.LearnableMoves[0].Base.Name}";
        move2Text.text = $"{pokemon.Base.LearnableMoves[1].Base.Name}";
        move3Text.text = $"{pokemon.Base.LearnableMoves[2].Base.Name}";
        move4Text.text = $"{pokemon.Base.LearnableMoves[3].Base.Name}";
    }
}
