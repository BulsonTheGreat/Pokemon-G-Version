using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    [SerializeField] Color highlightedColor;

    [SerializeField] Sprite normalType;
    [SerializeField] Sprite fireType;
    [SerializeField] Sprite waterType;
    [SerializeField] Sprite grassType;
    [SerializeField] Sprite electricType;
    [SerializeField] Sprite bugType;
    [SerializeField] Sprite poisonType;
    [SerializeField] Sprite groundType;
    [SerializeField] Sprite flyingType;
    [SerializeField] Sprite psychicType;
    [SerializeField] Sprite darkType;
    [SerializeField] Sprite ghostType;
    [SerializeField] Sprite fightingType;
    [SerializeField] Sprite rockType;
    [SerializeField] Sprite iceType;
    [SerializeField] Sprite steelType;
    [SerializeField] Sprite dragonType;
    [SerializeField] Sprite fairyType;

    Dictionary<PokemonTypes, Sprite> typeImages;

    public Color HighlightedColor => highlightedColor;

    public static GlobalSettings I { get; private set; }

    private void Awake()
    {
        I = this;

        typeImages = new Dictionary<PokemonTypes, Sprite>()
        {
            { PokemonTypes.None, null },
            { PokemonTypes.Normal, normalType },
            { PokemonTypes.Fire, fireType },
            { PokemonTypes.Water, waterType },
            { PokemonTypes.Grass, grassType },
            { PokemonTypes.Electric, electricType },
            { PokemonTypes.Bug, bugType },
            { PokemonTypes.Poison, poisonType },
            { PokemonTypes.Ground, groundType },
            { PokemonTypes.Flying, flyingType },
            { PokemonTypes.Psychic, psychicType },
            { PokemonTypes.Dark, darkType },
            { PokemonTypes.Ghost, ghostType },
            { PokemonTypes.Fighting, fightingType },
            { PokemonTypes.Rock, rockType },
            { PokemonTypes.Ice, iceType },
            { PokemonTypes.Steel, steelType },
            { PokemonTypes.Dragon, dragonType },
            { PokemonTypes.Fairy, fairyType }
        };
    }

    public Dictionary<PokemonTypes, Sprite> TypeImages => typeImages;

}
