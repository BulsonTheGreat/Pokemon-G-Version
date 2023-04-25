using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] PokemonTypes type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] MoveCategories moveCategorie;
    [SerializeField] MoveEffects effects;
    [SerializeField] List<SecondaryEffects> secondaryEffects;
    [SerializeField] MoveTargets target;
    [SerializeField] bool alwaysHits;
    [SerializeField] bool restoresHP;
    [SerializeField] bool hasRecoil;
    [SerializeField] bool extraCritChance;
    [SerializeField] bool priority;


    public string Name
    {
        get { return name; }
    }
    public string Description
    {
        get { return description; }
    }
    public PokemonTypes Type
    {
        get { return type; }
    }
    public int Power
    {
        get { return power; }
    }
    public int Accuracy
    {
        get { return accuracy; }
    }
    public MoveCategories MoveCategorie
    {
        get { return moveCategorie; }
    }
    public MoveEffects Effects
    {
        get { return effects; }
    }
    public List<SecondaryEffects> SecondaryEffects
    {
        get { return secondaryEffects; }
    }
    public MoveTargets Targets
    {
        get { return target; }
    }
    public bool AlwaysHits
    {
        get { return alwaysHits; }
    }
    public bool Priority
    {
        get { return priority; }
    }
    public bool RestoresHP
    {
        get { return restoresHP; }
    }
    public bool HasRecoil
    {
        get { return hasRecoil; }
    }
    public bool ExtraCrit
    {
        get { return extraCritChance; }
    }
}

[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] ConditionID status;
    [SerializeField] ConditionID volatileStatus;
    public List<StatBoost> Boosts
    {
        get { return boosts; }
    }
    public ConditionID Status
    {
        get { return status; }
    }
    public ConditionID VolatileStatus
    {
        get { return volatileStatus; }
    }
}
[System.Serializable]
public class SecondaryEffects : MoveEffects
{
    [SerializeField] int chance;
    [SerializeField] MoveTargets target;
    [SerializeField] bool selfTarget;
    public int Chance
    {
        get { return chance; }
    }
    public bool SelfTarget
    {
        get { return selfTarget; }
    }
    public MoveTargets Target
    {
        get { return target; }
    }
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}

public enum MoveCategories
{
    Physical,
    Special,
    Status,
}
public enum MoveTargets
{
    Foe,
    Self,
}
