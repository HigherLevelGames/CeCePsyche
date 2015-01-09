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
        SaveStateManager.data.CeciData = this;
        SpawnManager.PlayerTransform = PlayerTransform;
        SpawnableInventoryLibrary.data.SpawnTransform = ItemSpawnTransform;
    }
    public void LoadIn(CheckpointData cpd)
    {
        gameObject.transform.position = cpd.playerlocation;
        for (int i = 0; i < cpd.Items.Length; i++)
            inventory.Items.Add((ItemActions)cpd.Items [i]);
    }
}
