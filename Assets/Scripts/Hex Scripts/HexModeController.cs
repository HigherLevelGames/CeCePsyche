using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HexModeController : MonoBehaviour
{
    public Transform PlayerTransform;
    public int PlayerIndex = 0;
    public Sprite[] ItemSprites = new Sprite[7];
    public ItemActions[] Actions = new ItemActions[7];
    ItemInfo[] Info = new ItemInfo[7];
    Inventory inventory;
    ItemBehaviour[] Buttons;
    Animator animator;
    bool isMenuOpen, isMenuReady;
    int slotIndex;

    void Start()
    {
        inventory = InventoryManager.data.inventories [PlayerIndex]; 
        Buttons = GetComponentsInChildren<ItemBehaviour>();
        Debug.Log(Buttons.Length);
        animator = GetComponent<Animator>();
        for (int i = 0; i < Info.Length; i++)
        {
            Info [i].sprite = ItemSprites [i];
            Info [i].action = Actions [i];
        }
    }

    void Update()
    {
        if (isMenuOpen && isMenuReady)
        {
            if (RebindableInput.GetKey("LeftItem"))
            {
                slotIndex--;
                if (slotIndex < 0)
                    slotIndex = 5;
                animator.SetInteger("SlotIndex", slotIndex);
                animator.SetTrigger("TurnLeft");
                isMenuReady = false;
            }
            else if (RebindableInput.GetKey("RightItem"))
            {
                slotIndex++;
                if (slotIndex > 5)
                    slotIndex = 0;
                animator.SetInteger("SlotIndex", slotIndex);
                animator.SetTrigger("TurnRight");
                isMenuReady = false;
            }
        }
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

        if (RebindableInput.GetKeyDown("UseItem"))
            ;
    }

    public void SetReady()
    {
        isMenuReady = true;
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
        ItemInfo info;
        switch (item)
        {
            case ItemActions.Nothing:
                info = Info [0];
                break;
            case ItemActions.SqueakyToy:
                info = Info [1];
                break;
            case ItemActions.Dynamite:
                info = Info [2];
                break;
            case ItemActions.DogBone:
                info = Info [3];
                break;
            case ItemActions.StinkyPerfume:
                info = Info [4];
                break;
            case ItemActions.ZapNectar:
                info = Info [5];
                break;
            case ItemActions.Squirrel:
                info = Info [6];
                break;
            default:
                info = Info [0];
                break;
        }
        return info;
    }
}

public struct ItemInfo
{
    public Sprite sprite;
    public ItemActions action;
}






