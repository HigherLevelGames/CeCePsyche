using UnityEngine;
using System.Collections;

public class PortalToScene : MonoBehaviour, IInteractable
{
    public int sceneNumber = -1;
    public string sceneName = string.Empty;

    public void Interact()
    {
        GameStateManager.data.levelnum = sceneNumber;
        GameStateManager.data.levelname = sceneName;
        CanvasManager.data.StartTransition();
        Destroy(this.gameObject);
    }    
}
