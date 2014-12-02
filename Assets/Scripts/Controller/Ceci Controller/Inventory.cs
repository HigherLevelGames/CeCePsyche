using UnityEngine;
using System;
using System.Collections.Generic;


public class Inventory : MonoBehaviour
{
    public static GameObject[] ItemPrefabs;
    public List<ItemActions> Items = new List<ItemActions>();

    public void AddItem(ItemActions item)
    {
        Items.Add(item);
    }

    public GameObject GetItem(int idx)
    {
        if (idx < 0 || idx > Items.Count - 1)
        {
            Debug.Log(this.ToString() + " : Item index " + idx.ToString() + " not found.");
            return null;
        }
        return ItemPrefabs[(int)Items[idx]];
    }
    public GameObject[] GetItemObjects()
    {
        List<GameObject> temp = new List<GameObject>();
        for (int i = 0; i < Items.Count; i++)
        {
            temp.Add(GetItem(i));
        }
        return temp.ToArray();
    }
}


