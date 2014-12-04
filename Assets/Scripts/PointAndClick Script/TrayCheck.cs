using UnityEngine;
using System.Collections;

public class TrayCheck : MonoBehaviour
{

    void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i < InventoryManager.data.inventories [0].Items.Count)
            {

            } else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}
