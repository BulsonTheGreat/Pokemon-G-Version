using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonDB 
{
    static Dictionary<string, PokemonBasics> pokemon;

    public static void Init()
    {
        pokemon = new Dictionary<string, PokemonBasics>();

        var pkmnArray = Resources.LoadAll<PokemonBasics>("");
        foreach (var pkmn in pkmnArray)
        {
            if(pokemon.ContainsKey(pkmn.Name))
            {
                continue;
            }
            pokemon[pkmn.Name] = pkmn;
        }
    }
    public static PokemonBasics SearchForPkmn(string name)
    {
        if(!pokemon.ContainsKey(name))
        {
            Debug.Log("This pokemon doesn't exist");
            return null;
        }
        else
        {
            return pokemon[name];
        }
    }
}
