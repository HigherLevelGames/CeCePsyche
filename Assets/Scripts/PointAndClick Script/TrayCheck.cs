using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrayCheck : MonoBehaviour
{
    public void UpdateTray()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject slot = transform.GetChild(i).gameObject;
            if (i < InventoryManager.data.inventories [0].Items.Count)
            {
                slot.SetActive(true);
                Image img = slot.GetComponent<Image>();
                img.sprite = InventoryManager.data.inventories[0].GetItemSprite(i);
            } else
                slot.SetActive(false);
        }
    }

}
