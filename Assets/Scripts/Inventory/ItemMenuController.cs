using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemMenuController : MonoBehaviour
{
    // TODO: 
    // Find if mouse is in use and use mouse based selection instead of player key/button input.
    #region data
    public int PlayerIndex = 0;
    public GameObject CurrentItemDisplay;
    public GameObject[] Slots;
    Inventory inventory;
    RectTransform[] ItemTransforms;
    ItemBehaviour[] ItemScripts;
    Button[] ItemButtons;

    Vector3 Rotation
    {
        get { return transform.rotation.eulerAngles; }
        set { transform.rotation = Quaternion.Euler(value); }
    }

    Vector3 ChildRotations
    {
        get { return ItemTransforms [0].rotation.eulerAngles; }
        set
        {
            foreach (RectTransform r in ItemTransforms)
                r.rotation = Quaternion.Euler(value);
        }
    }

    Vector3 preTurnRotation;
    bool isMenuOpen, isMenuReady, turnLeft, turnRight, openMenu, closeMenu;
    float opentimer, turntimer;
    int slotIndex;
    #endregion

    void Start()
    {
        inventory = InventoryManager.data.inventories [PlayerIndex]; 

        ItemScripts = new ItemBehaviour[Slots.Length];
        ItemTransforms = new RectTransform[Slots.Length];
        ItemButtons = new Button[Slots.Length];
        for (int i = 0; i < Slots.Length; i++)
        {
            ItemTransforms [i] = (RectTransform)Slots [i].transform;
            ItemButtons [i] = Slots [i].GetComponent<Button>();
            ItemScripts [i] = Slots [i].GetComponent<ItemBehaviour>();
        }
    }

    void Update()
    {
        if (isMenuOpen && isMenuReady)
        {
            if (RebindableInput.GetKey("LeftItem"))
                TurnMenuLeft();
            else if (RebindableInput.GetKey("RightItem"))
                TurnMenuRight();
        }
        if (RebindableInput.GetKeyDown("Pause"))
        {
            if (isMenuOpen)
                CloseMenu();
            else
                OpenMenu();
        }

        if (RebindableInput.GetKeyDown("UseItem"))
        {
            if (isMenuOpen)
                CloseMenu();
            else
                ItemScripts [slotIndex].Use();
        }
        if (opentimer > 0)
        {
            opentimer -= Time.deltaTime;
            if (opentimer < 0)
            {
                isMenuReady = true;
                opentimer = 0;
            }
            MenuClosingAndOpening();
        }
        if (turntimer > 0)
        {
            turntimer -= Time.deltaTime * 4f;
            if (turntimer < 0)
            {
                turntimer = 0;
                isMenuReady = true;
            }
            MenuTurning();
        }
    }

    void TurnMenuLeft()
    {
        preTurnRotation = Rotation;
        slotIndex--;
        if (slotIndex == -1)
            slotIndex = Slots.Length - 1;
        
        turntimer = 1;
        turnLeft = true;
        turnRight = false;
        isMenuReady = false;
        ItemButtons [slotIndex].Select();
    }

    void TurnMenuRight()
    {
        preTurnRotation = Rotation;
        slotIndex++;
        if (slotIndex == Slots.Length)
            slotIndex = 0;
        turntimer = 1;
        turnRight = true;
        turnLeft = false;
        isMenuReady = false;
        ItemButtons [slotIndex].Select();
    }

    void OpenMenu()
    {
        opentimer = 1 - opentimer;
        isMenuOpen = true;
        closeMenu = false;
        openMenu = true;
        ItemButtons [slotIndex].Select();
        CurrentItemDisplay.GetComponent<Image>().sprite = InventoryManager.ItemsInfo [0].menusprite;
    }

    void CloseMenu()
    {
        opentimer = 1 - opentimer;
        closeMenu = true;
        openMenu = false;
        isMenuOpen = false;
        CurrentItemDisplay.GetComponent<Image>().sprite = InventoryManager.ItemsInfo [(int)ItemScripts [slotIndex].action].menusprite;
    }

    void MenuTurning()
    {
        float r = Mathf.Lerp(0, 60, 1 - turntimer);
        if (turnLeft)
            Rotation = preTurnRotation - new Vector3(0, 0, r);
        else if (turnRight)
            Rotation = preTurnRotation + new Vector3(0, 0, r);
        ChildRotations = new Vector3(0, 0, 0);
    }

    void MenuClosingAndOpening()
    {
        Rotation = new Vector3(0, 10, opentimer * 360 * 4f + (360 / Slots.Length) * slotIndex);
        if (openMenu)
        {
            transform.localScale = Vector3.one * (1 - opentimer);
        } else if (closeMenu)
        {
            transform.localScale = Vector3.one * opentimer;
        }
        ChildRotations = new Vector3(0, 0, 0);
    }

    public void ToggleMenuFromClick()
    {
        if (isMenuOpen)
            CloseMenu();
        else
            OpenMenu();
    }

    public void SetSlotAndClose(int slot)
    {
        slotIndex = slot;
        CloseMenu();
    }

    public void RefreshInventory()
    {
        for (int i = 0; i < ItemScripts.Length; i++)
        {
            if (i < inventory.Items.Count)
                ItemScripts [i].SetData(InventoryManager.ItemsInfo [(int)inventory.Items[i]]);
            else
                ItemScripts [i].SetData(InventoryManager.ItemsInfo [0]);
        }
		CurrentItemDisplay.GetComponent<Image>().sprite = InventoryManager.ItemsInfo [(int)ItemScripts [slotIndex].action].menusprite;
    }
}








