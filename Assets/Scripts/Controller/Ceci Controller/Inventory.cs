using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    List<ItemActions> Items;

    public void AddItem(ItemActions item)
    {
        Items.Add(item);
    }

    public void GetItem(int idx)
    {
        if (idx < 0 || idx > Items.Count - 1)
            return;
        switch (Items [idx])
        {
            case ItemActions.Nothing:
                return;
            case ItemActions.DogBone:
                break;
        }
    }
}


