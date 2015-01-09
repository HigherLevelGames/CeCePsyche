using UnityEngine;
using System.Collections;

// This enumeration lists out all of the possible transitions in the game 
// and keeps them in a "list" of sorts, there needs to be one item for each
// set of portals. ie. A portal in the Shire that leads to Mordor would need
// an item: ShireToMordor, which would then be used as the relation on BOTH
// portals. The program searches for it's paired portal and moves the player
// accordingly.
/// <summary>
/// The relation between a pair of portals used to determine spawn points.
/// </summary>
public enum PortalRelation
{
    None = -1,
    KitchenToHouse = 0,
    BedroomToHouse = 1,
    GardenToHouse = 2
}

public class PortalToScene : MonoBehaviour, IInteractable
{
    public int sceneNumber = -1;
    public string sceneName = string.Empty;
    public PortalRelation relation = PortalRelation.None;

    void Start()
    {
        if (relation != PortalRelation.None && relation == SpawnManager.relation)
            Game.Ceci.transform.position = this.gameObject.transform.position;
    }

    public void Interact()
    {
        GameStateManager.data.levelnum = sceneNumber;
        GameStateManager.data.levelname = sceneName;
        SpawnManager.relation = relation;
        CanvasManager.data.StartTransition();
        Destroy(this.gameObject);
    }    
}
