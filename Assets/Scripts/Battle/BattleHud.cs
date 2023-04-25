using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ConditionsDB;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] HpBar hpBar;
    [SerializeField] Text statusText;
    [SerializeField] Text hpText;
    [SerializeField] Color halfHp;
    [SerializeField] Color lowHp;

    [SerializeField] Color psnColor;
    [SerializeField] Color brnColor;
    [SerializeField] Color parColor;
    [SerializeField] Color frzColor;
    [SerializeField] Color slpColor;
    Pokemon _pokemon;
   
    Dictionary<ConditionID, Color> statusColors;
    //setting data to unit's HUD
    public void SetData(Pokemon pokemon)
    {
        gameObject.SetActive(true);
        _pokemon = pokemon;
        nameText.text = pokemon.Base.Name;
        hpText.text = $"{pokemon.HP}/{pokemon.MaxHP}";
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHP);
        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
        statusColors = new Dictionary<ConditionID, Color>()
        {
            { ConditionID.psn, psnColor },
            { ConditionID.brn, brnColor },
            { ConditionID.par, parColor },
            { ConditionID.frz, frzColor },
            { ConditionID.slp, slpColor },

        };
    }
    public void SetStatusText()
    {
        if(_pokemon.Status == null)
        {
            statusText.text = "";
        }
        else
        {
            statusText.text = _pokemon.Status.Id.ToString().ToUpper();
            statusText.color = statusColors[_pokemon.Status.Id];
        }
    }
    //reduce HP bar
    public IEnumerator UpdateHP()
    {
        if(_pokemon.HpChanged == true)
        {
            yield return hpBar.SetHpSmoothly((float)_pokemon.HP / _pokemon.MaxHP);
            hpText.text = $"{_pokemon.HP}/{_pokemon.MaxHP}";
            _pokemon.HpChanged = false;
        }
    }
}
