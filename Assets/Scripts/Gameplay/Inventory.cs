using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour, ISavable
{
    [SerializeField] List<ItemSlot> items;
    
    public List<ItemSlot> Items
    {
        get => items;
        set => items = value;
    }

    public object CaptureState()
    {
        var saveData = new InventorySaveData()
        {
            heldItems = items.Select(i => i.GetSaveData()).ToList(),
        };
        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = state as InventorySaveData;
        items = saveData.heldItems.Select(i => new ItemSlot(i)).ToList();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] ItemBasics item;

    public ItemBasics Base
    {
        get => item;
        set => item = value;
    }

    public ItemSlot(ItemSaveData saveData)
    {
        item = ItemDB.SearchForItem(saveData.itemName);
    }

    public ItemSaveData GetSaveData()
    {
        var saveData = new ItemSaveData()
        {
            itemName = item.Name,
        };
        return saveData;
    }
}

[Serializable]
public class ItemSaveData
{
    public string itemName;
}

[Serializable]
public class InventorySaveData
{
    public List<ItemSaveData> heldItems;
}

