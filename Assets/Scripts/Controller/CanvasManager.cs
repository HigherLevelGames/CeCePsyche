using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class CanvasManager : MonoBehaviour
{
    public static CanvasManager data;
    public Text DynamicText;
    public Text PromptText;
    public Text HintText;
    public Transition transitionScript;
    void Awake()
    {
        if (data == null)
            data = this;
    }
    public void StartTransition()
    {
        transitionScript.TransitionOut();
    }
}
