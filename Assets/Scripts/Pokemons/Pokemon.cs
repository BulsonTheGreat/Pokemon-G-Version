using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBasics _base;
    
    public PokemonBasics Base {
        get
        {
            return _base;
        }
    }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }
    public Move CurrentMove { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();
    public Conditions Status { get; private set; }
    public int StatusTime { get; set; }
    public int StatusDmg { get; set; }
    public bool HpChanged { get; set; }
    public event Action OnStatusChanged;
    public Conditions VolatileStatus { get; private set; }
    public int VolatileStatusTime { get; set; }

    //initializing a pokemon by giving it moves, definying it's stats and reseting any boosts
    public void Init()
    {
        //Generate moves
        Moves = new List<Move>();
        foreach(var move in Base.LearnableMoves)
        {
            Moves.Add(new Move(move.Base));
            if(Moves.Count >= 4)
            {
                break;
            }
        }
        CalculateStats();
        HP = MaxHP;
        StatusChanges = new Queue<string>();
        ResetStatBoosts();
        Status = null;
        VolatileStatus = null;
    }

    public Pokemon(PokemonSaveData saveData)
    {
        _base = PokemonDB.SearchForPkmn(saveData.name);
        HP = saveData.hp;
        CalculateStats();
        StatusChanges = new Queue<string>();
        ResetStatBoosts();
        Status = null;
        VolatileStatus = null;

        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            Moves.Add(new Move(move.Base));
            if (Moves.Count >= 4)
            {
                break;
            }
        }
    }
    //converts save data into SaveData class
    public PokemonSaveData GetSaveData()
    {
        var saveData = new PokemonSaveData()
        {
            name = Base.name,
            hp = HP,
        };
        return saveData;
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>
        {
            { Stat.Attack, Base.Attack },
            { Stat.Defense, Base.Defense  },
            { Stat.SpAttack, Base.SpAttack },
            { Stat.SpDefense, Base.SpDefense },
            { Stat.Speed, Base.Speed }
        };
        MaxHP = Base.MaxHP;
    }
    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];
        //Apply Stat Boost
        int boost = StatBoosts[stat];
        var boostValue = new float[] {1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
        {
            statVal = Mathf.FloorToInt(statVal * boostValue[boost]);
        }
        else
        {
            statVal = Mathf.FloorToInt(statVal / boostValue[-boost]);
        }
        return statVal;
    }
    //function for applying stat boosts
    public void ApplyBoost(List<StatBoost> statBoosts)
    {
        foreach(var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp((StatBoosts[stat] + boost), -6, 6);

            if(boost > 0)
            {
                StatusChanges.Enqueue($"{Base.Name}'s {stat} rose!");
            }
            else
            {
                StatusChanges.Enqueue($"{Base.Name}'s {stat} fell!");
            }

            Debug.Log($"{stat} has been boosted to {StatBoosts[stat]}");
        }
    }
    //burn is supposed to cut your attack in half so i just made it give a debuff
    public void BurnDebuff()
    {
        StatBoosts[Stat.Attack] = StatBoosts[Stat.Attack] - 2;
    }
    //paralysis lowers your speed so it got the same treatment as burn 
    public void PrlzDebuff()
    {
        StatBoosts[Stat.Speed] = StatBoosts[Stat.Speed] - 2;
    }
    //unfortunatly prlz and brn debuffs also reset while switching pokemon so it needs to be improved
    public void ResetStatBoosts()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0 },
            {Stat.Defense, 0 },
            {Stat.SpAttack, 0 },
            {Stat.SpDefense, 0 },
            {Stat.Speed, 0 },
            {Stat.Accuracy, 0 },
        };
    }
    //applying a status condition
    public void SetStatus(ConditionID conditionID)
    {
        bool isImmune = false; 
        foreach (var type in ConditionsDB.StatusConditions[conditionID].ImmuneTypes)
        {
            if (Base.Type1 == type || Base.Type2 == type)
            {
                isImmune = true;
            }
        }
        if (!isImmune)
        {
            Status = ConditionsDB.StatusConditions[conditionID];
            Status?.OnStart?.Invoke(this);
            StatusChanges.Enqueue($"{Base.Name} {Status.StartMessage}");
            OnStatusChanged?.Invoke();
        }
        else
        {
            StatusChanges.Enqueue("This effect doesn't work on opposing Pokemon!");
        }
    }
    
    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }
    //for confusion and flinching
    public void SetVolatileStatus(ConditionID conditionID)
    {
        VolatileStatus = ConditionsDB.StatusConditions[conditionID];
        VolatileStatus?.OnStart?.Invoke(this);
        if (VolatileStatus.StartMessage != null)
        {
            StatusChanges.Enqueue($"{Base.Name} {VolatileStatus.StartMessage}");
        } 
    }
    public void CureVolatileStatus()
    {
        VolatileStatus = null;
        OnStatusChanged?.Invoke();
    }
    //for things that can healyour units (like doctors)
    public void Heal()
    {
        HP = MaxHP;
        ResetStatBoosts();
        CureStatus();
        CureVolatileStatus();
        //OnHPChanged?.Invoke();
    }

    public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }
    public int Defense
    {
        get { return GetStat(Stat.Defense); }
    }
    public int SpAttack
    {
        get { return GetStat(Stat.SpAttack); }
    }
    public int SpDefense
    {
        get { return GetStat(Stat.SpDefense); }
    }
    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }
    public int MaxHP
    {
        get;
        private set;
    }
    //calculating damage taken from a move
    public DamageDetails TakeDamage(Move move, Pokemon attacker, Pokemon defender)
    {
        //small chance of landing a crit
        float critical = 1f;
        if(move.Base.ExtraCrit == false)
        {
            if(Random.value * 100f <= 6.25f)
            {
                critical = 1.5f;
            }
        }
        //or bigger if a move has this property
        else if(move.Base.ExtraCrit == true)
        {
            if (Random.value * 100f <= 12.5f)
            {
                critical = 1.5f;
            }
        }
        //based on the type matchup chart in PokemonBasics
        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false,
        };

        float attackType = attacker.Attack;
        float defenseType = defender.Defense;
        //deciding wheter to calculate based on physical stats (attack, defense) or special stats (spAttack, spDef)
        if (move.Base.MoveCategorie == MoveCategories.Special)
        {
            attackType = attacker.SpAttack;
            defenseType = defender.SpDefense;
        }
        else if(move.Base.MoveCategorie == MoveCategories.Physical)
        {
            attackType = attacker.Attack;
            defenseType = defender.Defense;
        }
        //if a move has the same type as the user it does some extra damage (stab = same type attack bonus)
        float stab = 1f;
        if(attacker.Base.Type1 == move.Base.Type || attacker.Base.Type2 == move.Base.Type)
        {
            stab = 1.2f;
        }
        //multiplying all the damage modifiers
        float modifiers = Random.Range(0.85f, 1f) * type * critical * stab;
        //damage formula
        int maxDamage = Mathf.FloorToInt((float)(0.4 * move.Base.Power * (attackType / defenseType) * modifiers));
        int damage = Mathf.Clamp(maxDamage, 0, HP);
        //Debug.Log($"Move does {damage} damage");
        if (move.Base.RestoresHP)
        {
            int fullHP = attacker.MaxHP - attacker.HP;
            int restored = Mathf.Clamp(damage / 2, 1, fullHP);
            attacker.RestoreHP(restored);
        }
        //apply recoil damage for some strong moves with this side effects
        if (move.Base.HasRecoil)
        {
            //just making sure that you can't die from recoil
            int recoilDMG = Mathf.Clamp(damage / 4, 0, attacker.HP - 1);
            attacker.UpdateHP(recoilDMG);
        }

        defender.UpdateHP(damage);
        return damageDetails;
    }
    //reducing HP
    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        Debug.Log($"Unit took {damage} damage");
        HpChanged = true;
    }
    //restoring HP
    public void RestoreHP(int amount)
    {
        HP = Mathf.Clamp(HP + amount, 1, MaxHP);
        Debug.Log($"Unit restored {amount} HP");
        HpChanged = true;
    }
    //for battles (to be expanded maybe)
    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
    //if a pokemon has a damaging status conditions apply the damage
    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
        VolatileStatus?.OnAfterTurn?.Invoke(this);
    }
    //usualy for conditions that may prevent you from doing anything (paralysis, sleep, freeze)
    public bool OnBeforeTurn()
    {
        bool canPerformMove = true;
        if (Status?.OnBeforeMove != null)
        {
            if (!Status.OnBeforeMove(this))
            {
                canPerformMove = false;
            }
        }
        if (VolatileStatus?.OnBeforeMove != null)
        {
            if (!VolatileStatus.OnBeforeMove(this))
            {
                canPerformMove = false;
            }
        }
        return canPerformMove;
    }
    //reset stat boosts and cure status effects after a battle ends
    public void OnBattleOver()
    {
        VolatileStatus = null;
        ResetStatBoosts();
        CureStatus();
        CureVolatileStatus();
    }
}
//class used for calculating damage
public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}

[System.Serializable]
public class PokemonSaveData
{
    public string name;
    public int hp;
}
