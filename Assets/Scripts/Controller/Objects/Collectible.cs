using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

    public ItemActions ItemType = ItemActions.TuningFork;

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.E))
            CollectedBy(other);
    }
	public void CollectedBy(Collider2D other)
    {
        Inventory inv = other.gameObject.GetComponent<Inventory>();
        inv.AddItem(ItemType);
        Destroy(this.gameObject);
    }
}
