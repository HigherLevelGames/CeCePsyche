using UnityEngine;
using System.Collections;

// This is a library of all the item prefabs that can be spawned from the player's inventory.
public class SpawnableInventoryLibrary : MonoBehaviour
{
    public static SpawnableInventoryLibrary data;
    [HideInInspector]
    public Transform SpawnTransform;

    public GameObject SqueakyToy;

    void Awake()
    {
        if (data == null)
            data = this;
    }

    void OnLevelWasLoaded(int level)
    {
        //SpawnPoint = FindObjectOfType<PlayerInput>().gameObject.transform;
    }
}
