using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

    public ItemActions ItemType;

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.E))
            Collect(other);
    }
	public void Collect(Collider2D other)
    {
        Inventory inv = other.gameObject.GetComponent<Inventory>();
        inv.AddItem(ItemType);
    }
}
