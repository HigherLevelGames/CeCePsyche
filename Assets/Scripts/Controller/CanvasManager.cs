using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager data;
    public TrayCheck Tray;
    public Image ObjectImage;
    public Text ObjectText;
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
    }

    void Start()
    {
        Tray.UpdateTray();
    }

    public void StartTransition()
    {
        transitionScript.TransitionOut();
    }
}
