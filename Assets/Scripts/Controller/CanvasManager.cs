using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class CanvasManager : MonoBehaviour
{
    public static CanvasManager data;
    public TrayCheck Tray;
    public Image BookImage;
    public Text BookText;
    public Image KeyImage;
    public Text KeyText;
    public Text DynamicText;
    public Text PromptText;
    public Text HintText;
    public Transition transitionScript;
    void Awake()
    {
        if (data == null)
            data = this;
        Tray.UpdateTray();
    }
    public void StartTransition()
    {
        transitionScript.TransitionOut();
    }
}
