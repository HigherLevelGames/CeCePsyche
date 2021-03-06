﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ItemInfo
{
    public Sprite menusprite;
    public ItemActions action;
}

public enum ItemActions
{
    Nothing = 0,
    SqueakyToy = 1, // plays a note and gets the dog's attention
    Dynamite = 2,
    DogBone = 3,
    StinkyPerfume = 4,
    ZapNectar = 5,
    Squirrel = 6
}

public enum CollectedFlags
{
    DoNotRemove = 0,
    SqueakyToy = 1,
    DogTreats = 2
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager data;
    public static ItemInfo[] ItemsInfo;
    public ItemMenuController ItemMenu;
    public Sprite[] ItemMenuSprites;
    public Inventory[] inventories;

    public int[] GetFlags()
    {
        int[] temp = new int[collected.Count];
        for (int i = 0; i < temp.Length; i++)
            temp [i] = (int)collected [i];
        return temp;
    }

    List<CollectedFlags> collected = new List<CollectedFlags>();
    Collectible[] itemsInScene;
    #region Jason Code Kingdom
    void Awake()
    {
        if (data == null)
        {
            data = this;
            inventories = new Inventory[1];
            inventories [0] = new Inventory();
            // Creates an informational object for each item sprite supplied to this manager
            ItemsInfo = new ItemInfo[ItemMenuSprites.Length];
            for (int i = 0; i < ItemsInfo.Length; i++)
            {
                ItemsInfo [i].menusprite = ItemMenuSprites [i];
                ItemsInfo [i].action = (ItemActions)i; // assigns actions by enum order, place sprites accordingly.
            }
        }
    }

    void OnEnable()
    {
        // Find all items placed in this scene
        itemsInScene = FindObjectsOfType<Collectible>(); // Load from serialized data for better performance
        // Remove already collected items from the scene.
        for (int i = 0; i < collected.Count; i++)
        {
            for (int j = 0; j < itemsInScene.Length; j++)
            {
                if (collected [i] == itemsInScene [j].Flag)
                    itemsInScene [j].gameObject.SetActive(false);
            }
        }
    }

    public void AddItemToInventory(int inventoryIndex, ItemActions item, CollectedFlags flag)
    {
        if ((int)flag > -1)
            collected.Add(flag);
        inventories [inventoryIndex].AddItem(item);
        CanvasManager.data.Tray.UpdateTray();
        ItemMenu.RefreshInventory();
    }

    public void ReplaceInventory(ItemActions[] items, CollectedFlags[] flags)
    {
        inventories [0].Items.Clear();
        collected.Clear();
        inventories [0].Items.AddRange(items);
        collected.AddRange(flags);
        CanvasManager.data.Tray.UpdateTray();
        ItemMenu.RefreshInventory();
    }
    #endregion
    #region Jodan Code Land
    // Unconditioned Stimuli will be thrown
    // and used to condition an unconditioned response.
    // ("UseItem" stored in inventory)
    //
    // Conditioned Stimuli will be part of the environment
    // and will illicit no response.
    // Once the subject has been conditioned,
    // then the environment can illicit the conditioned response.
    // ("Interact" with environment)
    /*
    public class Item
    {
        public string name = "";
        public int amount = 1;
        public GameObject itemObj;
        public Item(string n, GameObject obj)
        {
            name = n;
            itemObj = obj;
        }
    }
    public GameObject TreatObj;
    List<Item> inventory = new List<Item>();
    int curItem = 0;

    void Start() { }

    void Update()
    {
        if(inventory.Count == 0)
        {
            return;
        }

        if(RebindableInput.GetKeyDown("LeftItem"))
        {
            curItem = (curItem+inventory.Count-1) % inventory.Count;
        }
        if(RebindableInput.GetKeyDown("RightItem"))
        {
            curItem = (curItem+1) % inventory.Count;
        }
        if(RebindableInput.GetKeyDown("UseItem"))
        {
            UseItem(curItem);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0,0,100,50), (inventory.Count == 0) ? "Empty Inventory" :
                  inventory[curItem].name + ": " + inventory[curItem].amount);
    }

    void UseItem(int item)
    {
        // Error Checking
        if(inventory.Count == 0 || inventory.Count < item)
        {
            return;
        }
        if(inventory[item].amount <= 0)
        {
            return;
        }

        // Get direction ceci is facing to figure out where item should spawn
        Vector3 pos = this.transform.position;
        MovementController hControl = this.GetComponent<MovementController>();
        pos += new Vector3(hControl.isFacingRight? 1.5f:-1.5f,0.0f,0.0f);

        // Remove an item from inventory
        inventory[item].amount--;

        // Spawn the item
        GameObject stuff = (GameObject)Instantiate(inventory[item].itemObj,pos,this.transform.rotation);
        stuff.name = stuff.name.Substring(0,stuff.name.Length-7); // remove "(Clone)" at end of name
        stuff.SetActive(true);

        // Different items used for specific things.
        stuff.SendMessage("UseMe", hControl.isFacingRight, SendMessageOptions.DontRequireReceiver);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Collectable")
        {
            // collect the item, e.g. memories, neurons, emotion items, etc.
            //PlayerPrefs.SetInt(col.gameObject.name, PlayerPrefs.GetInt(col.gameObject.name) + 1);

            // Check the inventory to see if there's an item that already exists
            bool found = false;
            for(int i = 0; i < inventory.Count; i++)
            {
                if(inventory[i].name == col.gameObject.name)
                {
                    // Add item to already existing items
                    inventory[i].amount++;
                    Destroy(col.gameObject);
                    found = true;
                    break;
                }
            }

            // Item not found in inventory
            if(!found)
            {
                // Create new item and add it to the inventory
                Item newItem = new Item(col.gameObject.name, col.gameObject);
                inventory.Add(newItem);
                col.gameObject.SetActive(false);
            }
        }
    }

    // When CeCe gets close enough to an interactable object, i.e. she enter's the object's trigger zone
    void OnTriggerStay2D(Collider2D col)
    {
        // TODO: Jason's particle system or something that helps the player know the object is interactable
        
        // pressed interact key
        if(RebindableInput.GetKeyDown("Interact") && col.gameObject.tag == "Interactable")
        {
            // tell the other object to perform some action, e.g. open doors, treasure chests, use item/switches, etc.
            col.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
        }
    }
 */   
    #endregion

}
