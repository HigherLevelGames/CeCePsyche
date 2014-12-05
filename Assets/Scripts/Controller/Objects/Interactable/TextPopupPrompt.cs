using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextPopupPrompt : InGameButtonPrompt
{

    public Text DynamicText;
    public string Text;
    bool isOpen;
    Vector2 viewportPosition;

    void Start()
    {
        DynamicText.text = string.Empty;
    }

    void Update()
    {
        if (renderer.enabled) 
            if (RebindableInput.GetKey("Interact"))
            OpenText();
        PositionText();
    }

    void PositionText()
    {
        if (isOpen)
        {
            viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            viewportPosition.y += 0.1f;
            viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0.1f, 0.9f);
            float x1 = Mathf.Max(-0.1f, viewportPosition.x - 0.45f);
            float x2 = Mathf.Min(1.1f, viewportPosition.x + 0.45f);
            DynamicText.rectTransform.anchorMin = new Vector2(x1, viewportPosition.y - 0.2f);
            DynamicText.rectTransform.anchorMax = new Vector2(x2, viewportPosition.y + 0.2f);
            DynamicText.rectTransform.offsetMin = Vector2.zero;
            DynamicText.rectTransform.offsetMax = Vector2.zero;
        }
    }

    void OpenText()
    {
        if (isOpen)
        {
            isOpen = false;
            DynamicText.text = string.Empty;
        } else
        {
            isOpen = true;
            DynamicText.text = Text;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        DynamicText.text = string.Empty;
        isOpen = false;
        Deprompt();
    }
}