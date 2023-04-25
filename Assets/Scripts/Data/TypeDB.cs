using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TypeDB: MonoBehaviour
{
    [SerializeField] static Image normalType;
    [SerializeField] static Image fireType;
    [SerializeField] static Image waterType;
    [SerializeField] static Image grassType;
    [SerializeField] static Image electricType;
    [SerializeField] static Image bugType;
    [SerializeField] static Image poisonType;
    [SerializeField] static Image groundType;
    [SerializeField] static Image flyingType;
    [SerializeField] static Image psychicType;
    [SerializeField] static Image darkType;
    [SerializeField] static Image ghostType;
    [SerializeField] static Image fightingType;
    [SerializeField] static Image rockType;
    [SerializeField] static Image iceType;
    [SerializeField] static Image steelType;
    [SerializeField] static Image dragonType;
    [SerializeField] static Image fairyType;

    private void Start()
    {
        Dictionary<PokemonTypes, Image> ImageTypes = new Dictionary<PokemonTypes, Image>
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
}
