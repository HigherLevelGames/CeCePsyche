using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingTextPrompt : InGameButtonPrompt
{
    
    Text dynamicText;
    public string Text;
    bool isOpen;
    Vector2 viewportPosition;

    void Start()
    {
        dynamicText = CanvasManager.data.DynamicText;
        dynamicText.text = string.Empty;
    }
    
    void Update()
    {
        if (renderer.enabled) 
        if (RebindableInput.GetKey("Interact"))
            Interact();
        PositionText();
    }

    void Interact()
    {
        IInteractable inter = (IInteractable)GetComponent(typeof(IInteractable));
        inter.Interact();
        dynamicText.text = string.Empty;
        isOpen = false;
    }

    void PositionText()
    {
        if (isOpen)
        {

            viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            viewportPosition.y += 0.1f;
            viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0.1f, 0.9f);
            float x1 = viewportPosition.x - 0.45f;
            float x2 = viewportPosition.x + 0.45f;
            dynamicText.rectTransform.anchorMin = new Vector2(x1, viewportPosition.y - 0.2f);
            dynamicText.rectTransform.anchorMax = new Vector2(x2, viewportPosition.y + 0.2f);
            dynamicText.rectTransform.offsetMin = Vector2.zero;
            dynamicText.rectTransform.offsetMax = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        dynamicText.text = Text;
        isOpen = true;
        Prompt();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        dynamicText.text = string.Empty;
        isOpen = false;
        Deprompt();
    }
}