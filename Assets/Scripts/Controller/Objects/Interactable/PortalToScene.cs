using UnityEngine;
using System.Collections;

public class PortalToScene : MonoBehaviour, IInteractable
{
    public int sceneNumber = -1;
    public string sceneName = string.Empty;

    public void Interact()
    {
        GoToNextArea();
    }

    void GoToNextArea()
    {
        if (sceneNumber > -1)
        {
            Application.LoadLevel(sceneNumber);
            return;
        } else
        {
            if (sceneName != string.Empty)
            {
                Application.LoadLevel(sceneName);
                return;
            }
            Debug.Log("The level was unable to be loaded due to impossible load name or number");
        }

    }
    
}
