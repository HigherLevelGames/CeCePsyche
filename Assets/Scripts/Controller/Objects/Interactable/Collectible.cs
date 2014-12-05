using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour, IInteractable
{
    public ItemActions ItemType = ItemActions.SqueakyToy;
    public CollectedFlags Flag = CollectedFlags.DoNotRemove;
    void Awake()
    {
        for(int i = 0; i < InventoryManager.data.inventories[0].Items.Count; i++)
            if(InventoryManager.data.inventories[0].Items[i] == ItemType)
                Destroy(this.gameObject);
    }
    public void Interact()
    {
        Collect();
    }

    public void Collect()
    {
        InventoryManager.data.AddItemToInventory(0, ItemType);
        Destroy(this.gameObject);
    }
}
