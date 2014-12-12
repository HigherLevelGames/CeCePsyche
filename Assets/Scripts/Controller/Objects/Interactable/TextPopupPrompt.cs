using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextPopupPrompt : InGameButtonPrompt
{
    public string Text;
    Text DynamicText;
    Vector2 viewportPosition;
    bool showText;

    void Start()
    {
        DynamicText = CanvasManager.data.DynamicText;
        DynamicText.text = string.Empty;
    }

    void Update()
    {
        if (isOpen)
        { 
            if (RebindableInput.GetKeyDown("Interact"))
            {
                showText = true;
                Debug.Log(isOpen);
                Activate();
                DynamicText.text = Text;
            }
            PositionPrompt();
        } else
        {
            if (showText)
            {
                if (RebindableInput.GetKeyDown("Interact"))
                {
                    Deactivate();
                    showText = false;
                    DynamicText.text = string.Empty;
                }
                PositionPrompt();
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        DynamicText.text = string.Empty;
        showText = false;
        Deprompt();
    }
}