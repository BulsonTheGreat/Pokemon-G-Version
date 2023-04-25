using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static void Init()
    {
        foreach(var kvp in StatusConditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;
            condition.Id = conditionId;
        }
    }
    public static Dictionary<ConditionID, Conditions> StatusConditions { get; set; } = new Dictionary<ConditionID, Conditions>()
    {
        //defining effects of various status conditions
        {
            ConditionID.psn,
            new Conditions()
            {
                Name = "Poison",
                ImmuneTypes = new List<PokemonTypes>(){PokemonTypes.Poison, PokemonTypes.Steel },
                StartMessage = "has been poisoned",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.StatusDmg = 1;
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    pokemon.StatusDmg++;
                    return true;
                },
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    int poisonDmg = (pokemon.StatusDmg * pokemon.MaxHP)/16;
                    pokemon.UpdateHP(poisonDmg);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is hurt by poison");
                },
                VisualEffect = (BattleUnit unit) =>
                {
                    unit.PlayPoisonAnimation();
                }
            }
        },
        {
            ConditionID.brn,
            new Conditions()
            {
                Name = "Burn",
                ImmuneTypes = new List<PokemonTypes>(){PokemonTypes.Fire },
                StartMessage = "has been burned",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.BurnDebuff();
                },
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHP/16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is hurt by burn");
                },
                VisualEffect = (BattleUnit unit) =>
                {
                    unit.PlayBurnAnimation();
                }
            }
        },
        {
            ConditionID.par,
            new Conditions()
            {
                Name = "Paralysis",
                ImmuneTypes = new List<PokemonTypes>(){PokemonTypes.Electric },
                StartMessage = "is paralyzed, it may not move",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.PrlzDebuff();
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(Random.Range(1,5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is paralyzed, it can't move");
                        return false;
                    }
                    return true;
                },
                VisualEffect = (BattleUnit unit) =>
                {
                    unit.PlayPrlzAnimation();
                }
            }
        },
        {
            ConditionID.frz,
            new Conditions()
            {
                Name = "Freeze",
                ImmuneTypes = new List<PokemonTypes>(){PokemonTypes.Ice },
                StartMessage = "is frozen, it can't move",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(Random.Range(1,5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} thawed out");
                        pokemon.CureStatus();
                        return true;
                    }
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is frozen solid");
                    return false;
                },
                VisualEffect = (BattleUnit unit) =>
                {
                    unit.PlayFrzAnimation();
                }
            }
        },
        {
            ConditionID.slp,
            new Conditions()
            {
                Name = "Sleep",
                ImmuneTypes = new List<PokemonTypes>(){},
                StartMessage = "is fast asleep",
                OnStart = (Pokemon pokemon) =>
                {
                    //sleep for 1-3 turns
                    pokemon.StatusTime = Random.Range(1,4);
                    Debug.Log($"Will be asleep for {pokemon.StatusTime} turns");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.StatusTime <= 0)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} woke up");
                        pokemon.CureStatus();
                        return true;
                    }
                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is fast asleep");
                    return false;
                },
                VisualEffect = (BattleUnit unit) =>
                {
                    unit.PlaySlpAnimation();
                }
            }
        },
        {
            ConditionID.flinch,
            new Conditions()
            {
                Name = "Flinch",
                ImmuneTypes = new List<PokemonTypes>(){},
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.VolatileStatusTime = 1;
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.VolatileStatusTime == 0)
                    {
                        pokemon.CureVolatileStatus();
                        return true;
                    }
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} flinched");
                    pokemon.VolatileStatusTime--;
                    return false;
                },
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    if(pokemon.VolatileStatusTime == 1)
                    {
                        pokemon.VolatileStatusTime--;
                    }
                }
            }
        },
        {
            ConditionID.conf,
            new Conditions()
            {
                Name = "Confusion",
                StartMessage = "is confused",
                ImmuneTypes = new List<PokemonTypes>(){},
                OnStart = (Pokemon pokemon) =>
                {
                    //sleep for 1-3 turns
                    pokemon.VolatileStatusTime = Random.Range(2,5);
                    Debug.Log($"Will be confused for {pokemon.VolatileStatusTime} turns");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} snapped out of confusion!");
                        pokemon.CureVolatileStatus();
                        return true;
                    }
                    pokemon.VolatileStatusTime--;
                    if(Random.Range(1,3) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is confused");
                        return true;
                    }
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is confused");
                    pokemon.UpdateHP(pokemon.MaxHP/8);
                    pokemon.StatusChanges.Enqueue("It hurt itself in it's confusion");
                    return false;
                },
                VisualEffect = (BattleUnit unit) =>
                {
                    unit.PlayHitAnimation();
                }
            }
        },
    };
}

public enum ConditionID
{
    none, psn, brn, par, slp, frz, conf, flinch //poison, burn, paralyze, sleep, freeze, confusion, flinching
}
