using UnityEngine;
using System;
using System.Collections.Generic;


public class Inventory
{
    public List<ItemActions> Items = new List<ItemActions>();
    public Inventory()
    {

    }
    public void AddItem(ItemActions item)
    {
        Items.Add(item);
    }

    public Sprite GetItemSprite(int idx)
    {
        if (idx < 0 || idx > Items.Count - 1)
        {
            Debug.Log(this.ToString() + " : Item index " + idx.ToString() + " not found.");
            return null;
        }
        return InventoryManager.data.ItemSprites[(int)Items[idx]];
    }
}


