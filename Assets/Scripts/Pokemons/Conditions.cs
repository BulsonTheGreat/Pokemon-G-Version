using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConditionsDB;

public class Conditions
{
    public ConditionID Id { get;  set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }
    //for status conditions which take effect before a move is used (prevent from using it)
    public Func<Pokemon, bool> OnBeforeMove { get; set; }
    //for conditions which damage the target after each turn
    public Action<Pokemon> OnAfterTurn { get; set; }
    //for dictating how log an effect will last
    public Action<Pokemon> OnStart { get; set; }
    //for displaying visual effects of status conditions
    public Action<BattleUnit> VisualEffect { get; set; }
    //to do
    public List<PokemonTypes> ImmuneTypes { get; set; }
}
