using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PokemonBasics : ScriptableObject
{
    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;
    //sprites
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;
    [SerializeField] Sprite menuSprite;
    //types (max2)
    [SerializeField] PokemonTypes type1;
    [SerializeField] PokemonTypes type2;
    //base stats
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
    //int level = 100;
    //adding a list of moves
    [SerializeField] List<LearnableMoves> learnableMoves;
    public string Name
    {
        get { return name; }
    }
    public string Description
    {
        get { return description; }
    }
    public Sprite BackSprite
    {
        get { return backSprite; }
    }
    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }
    public Sprite MenuSprite
    {
        get { return menuSprite; }
    }
    public PokemonTypes Type1
    {
        get { return type1; }
    }
    public PokemonTypes Type2
    {
        get { return type2; }
    }
    public int MaxHP
    {
        get { return maxHP; }
    }
    public int Attack
    {
        get { return attack; }
    }
    public int Defense
    {
        get { return defense; }
    }
    public int SpAttack
    {
        get { return spAttack; }
    }
    public int SpDefense
    {
        get { return spDefense; }
    }
    public int Speed
    {
        get { return speed; }
    }
    public List<LearnableMoves> LearnableMoves
    {
        get { return learnableMoves; }
    }
}

[System.Serializable]
public class LearnableMoves
{
    [SerializeField] MoveBase moveBase;
    public MoveBase Base
    {
        get { return moveBase; }
    }
}

public enum PokemonTypes
{
    None,
    Normal,
    Fire,
    Water,
    Grass,
    Electric,
    Bug,
    Poison,
    Ground,
    Flying,
    Psychic,
    Dark,
    Ghost,
    Fighting,
    Rock,
    Ice,
    Steel,
    Dragon,
    Fairy,
}

public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,
    //These aren't actual stats, but they're used for moves
    Accuracy,
}

public class TypeChart
{
    //New balance patch: bug is super effective against fairy, grass resists fairy, poison is super effective against water,
    //                   ice resists ground and flying (because.....reasons)
    static readonly float[][] chart =
    {
        /*                   NOR  FIR   WAT   GRS   ELE   BUG   POI   GRO   FLY   PSY   DAR   GHO   FIG   ROC   ICE   STE   DRA   FAI */
        /*NOR*/ new float[] { 1f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0f,   1f,   0.5f, 1f,   0.5f, 1f,   1f   },
        /*FIR*/ new float[] { 1f, 0.5f, 0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 2f,   2f,   0.5f, 1f   },
        /*WAT*/ new float[] { 1f, 2f,   0.5f, 0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   0.5f, 1f   },
        /*GRS*/ new float[] { 1f, 0.5f, 2f,   0.5f, 1f,   0.5f, 0.5f, 2f,   0.5f, 1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 0.5f, 1f   },
        /*ELE*/ new float[] { 1f, 1f,   2f,   0.5f, 0.5f, 1f,   1f,   0f,   2f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f   },
        /*BUG*/ new float[] { 1f, 0.5f, 1f,   2f,   1f,   1f,   0.5f, 1f,   0.5f, 2f,   2f,   0.5f, 0.5f, 1f,   1f,   0.5f, 1f,   2f   },
        /*POI*/ new float[] { 1f, 1f,   2f,   2f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   1f,   0.5f, 1f,   0.5f, 1f,   0f,   1f,   2f   },
        /*GRO*/ new float[] { 1f, 2f,   1f,   0.5f, 2f,   0.5f, 2f,   1f,   0f,   1f,   1f,   1f,   1f,   2f,   0.5f, 2f,   1f,   1f   },
        /*FLY*/ new float[] { 1f, 1f,   1f,   2f,   0.5f, 2f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 0.5f, 0.5f, 1f,   1f   },
        /*PSY*/ new float[] { 1f, 1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   0.5f, 0f,   1f,   2f,   1f,   1f,   0.5f, 1f,   1f   },
        /*DAR*/ new float[] { 1f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 2f,   0.5f, 1f,   1f,   1f,   1f,   0.5f },
        /*GHO*/ new float[] { 0f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 2f,   1f,   1f,   1f,   1f,   1f,   1f   },
        /*FIG*/ new float[] { 2f, 1f,   1f,   1f,   1f,   0.5f, 0.5f, 1f,   0.5f, 0.5f, 2f,   0f,   1f,   2f,   2f,   2f,   1f,   0.5f },
        /*ROC*/ new float[] { 1f, 2f,   1f,   1f,   1f,   2f,   1f,   0.5f, 2f,   1f,   1f,   1f,   0.5f, 1f,   2f,   0.5f, 1f,   1f   },
        /*ICE*/ new float[] { 1f, 0.5f, 0.5f, 2f,   1f,   1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   1f,   0.5f, 0.5f, 2f,   1f   },
        /*STE*/ new float[] { 1f, 0.5f, 0.5f, 1f,   0.5f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   0.5f, 1f,   2f   },
        /*DRA*/ new float[] { 1f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 2f,   0f   },
        /*FAI*/ new float[] { 1f, 0.5f, 1f,   0.5f, 1f,   1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   2f,   1f,   1f,   0.5f, 2f,   1f   }
    };
    //getting a type effectivness of a move based on it's type and users type(s) (4x, 2x, 1x, 0.5x, 0.25x, 0)
    public static float GetEffectiveness(PokemonTypes attackerType, PokemonTypes defenderType)
    {
        if(attackerType == PokemonTypes.None || defenderType == PokemonTypes.None)
        {
            return 1;
        }
        int row = (int)attackerType - 1;
        int col = (int)defenderType - 1;
        return chart[row][col];
    }
}