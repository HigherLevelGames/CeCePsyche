using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour, IInteractable
{
    public ItemActions ItemType = ItemActions.TuningFork;

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
