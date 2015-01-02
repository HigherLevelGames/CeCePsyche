using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour, IInteractable
{
    public ItemActions ItemType = ItemActions.SqueakyToy;
    public CollectedFlags Flag = CollectedFlags.DoNotRemove;

    public void Interact()
    {
        Collect();
    }

    public void Collect()
    {
        InventoryManager.data.AddItemToInventory(0, ItemType, Flag);
        this.gameObject.SetActive(false); //?
    }
}
