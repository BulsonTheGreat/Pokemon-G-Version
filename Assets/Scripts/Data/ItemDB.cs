using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB
{
    static Dictionary<string, ItemBasics> items;

    public static void Init()
    {
        items = new Dictionary<string, ItemBasics>();

        var itemArray = Resources.LoadAll<ItemBasics>("");
        foreach (var itm in itemArray)
        {
            if (items.ContainsKey(itm.Name))
            {
                continue;
            }
            items[itm.Name] = itm;
        }
    }
    public static ItemBasics SearchForItem(string name)
    {
        if (!items.ContainsKey(name))
        {
            Debug.Log("This item doesn't exist");
            return null;
        }
        else
        {
            return items[name];
        }
    }
}
