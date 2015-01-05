using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrayCheck : MonoBehaviour
{
    GameObject[] slots;
    bool firstUpdate = true;
    public void UpdateTray()
    {
        if (firstUpdate)
        {
            slots = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                slots [i] = transform.GetChild(i).gameObject;
            }
            gameObject.SetActive(false);
            firstUpdate = false;
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < InventoryManager.data.inventories [0].Items.Count)
            {
                slots [i].SetActive(true);
                Image img = slots [i].GetComponent<Image>();
                img.sprite = InventoryManager.data.inventories [0].GetItemSprite(i);
            } else
                slots [i].SetActive(false);
        }
    }
}