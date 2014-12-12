using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingTextPrompt : InGameButtonPrompt
{
    public string Text;

    Text dynamicText;
    Vector2 viewportPosition;

    void Start()
    {
        dynamicText = CanvasManager.data.DynamicText;
        dynamicText.text = string.Empty;
    }
    void Update()
    {
        if (isOpen)
        {
            if (RebindableInput.GetKeyDown("Interact"))
            {
                Interact();
                return;
            }
            PositionPrompt();
        }
    }

    void Interact()
    {
        IInteractable inter = (IInteractable)GetComponent(typeof(IInteractable));
        inter.Interact();
        dynamicText.text = string.Empty;
        Activate();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        dynamicText.text = Text;
        Prompt();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        dynamicText.text = string.Empty;
        Deprompt();
    }
}