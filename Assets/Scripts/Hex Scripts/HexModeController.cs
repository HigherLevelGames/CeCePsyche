using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HexModeController : MonoBehaviour
{
    public Transform PlayerTransform;
    public int PlayerIndex = 0;
    public GameObject[] ItemPrefabs = new GameObject[7];
    public ItemActions[] Actions = new ItemActions[7];

    ItemInfo[] Info = new ItemInfo[7];
    Inventory inventory;
    ItemBehaviour[] Buttons;
    Animator animator;
    bool isMenuOpen;
    int slotIndex;
    void Start()
    {
        inventory = InventoryManager.data.inventories [PlayerIndex]; 
        Buttons = GetComponentsInChildren<ItemBehaviour>();
        animator = GetComponent<Animator>();
        for (int i = 0; i < Info.Length; i++)
        {
            Info [i].prefab = ItemPrefabs [i];
            Info [i].action = Actions [i];
        }
    }

    void Update()
    {
        if (RebindableInput.GetKeyDown("Pause"))
        {
            if (isMenuOpen)
            {
                animator.SetTrigger("TrigCloseMenu");
                isMenuOpen = false;
            } else
            {
                animator.SetTrigger("TrigOpenMenu");
                isMenuOpen = true;
            }
        }
        if (RebindableInput.GetKeyDown("LeftItem"))
        {
            slotIndex--;
            if(slotIndex < 0)
                slotIndex = 5;
            float a = ((float)slotIndex * 60f) / 360f;
            animator.SetFloat("TurnAmount", a);
        }
        if (RebindableInput.GetKeyDown("RightItem"))
        {
            slotIndex++;
            if(slotIndex > 5)
                slotIndex = 0;
            float a = ((float)slotIndex * 60f) / 360f;
            animator.SetFloat("TurnAmount", a);
        }
        if (RebindableInput.GetKeyDown("UseItem"))
            ;
    }

    public void RefreshInventory()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (i < inventory.Items.Count)
                Buttons [i].SetData(FindButtonData(inventory.Items [i]));
            else
                Buttons [i].SetData(FindButtonData(ItemActions.Nothing));
        }
    }

    ItemInfo FindButtonData(ItemActions item)
    {
        /*
public enum ItemActions
{
    Nothing = -1,
    SqueakyToy = 0, 
    Dynamite = 1,
    DogBone = 2,
    StinkyPerfume = 3,
    ZapNectar = 4,
    Squirrel = 5
}
         */
        switch (item)
        {
            case ItemActions.Nothing:
                return Info [0];
            case ItemActions.SqueakyToy:
                return Info [1];
            case ItemActions.Dynamite:
                return Info [2];
            case ItemActions.DogBone:
                return Info [3];
            case ItemActions.StinkyPerfume:
                return Info [4];
            case ItemActions.ZapNectar:
                return Info [5];
            case ItemActions.Squirrel:
                return Info [6];
        }
        return Info [0];
    }
}

public struct ItemInfo
{
    public GameObject prefab;
    public ItemActions action;
}






