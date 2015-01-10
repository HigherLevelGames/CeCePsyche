using UnityEngine;
using System.Collections;

// A behaviour used to check in player data to static classes for spawning and saving
public class PlayerDataCheckin : MonoBehaviour
{

    public Transform PlayerTransform;
    public Transform ItemSpawnTransform;
    public Inventory inventory;

    void Start()
    {
        inventory = InventoryManager.data.inventories [0];
        SaveStateManager.data.CeciData = this;
        SpawnManager.PlayerTransform = PlayerTransform;
        SpawnableInventoryLibrary.data.SpawnTransform = ItemSpawnTransform;
    }
    public void LoadIn(CheckpointData cpd)
    {
        gameObject.transform.position = new Vector3(cpd.xlocation, cpd.ylocation, 0);
    }
}
