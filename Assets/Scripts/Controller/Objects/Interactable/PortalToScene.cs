using UnityEngine;
using System.Collections;
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
    void Awake()
    {
        if (relation != PortalRelation.None)
        if(relation == SpawnManager.relation)
            SpawnManager.PlayerTransform.position = this.gameObject.transform.position;
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
