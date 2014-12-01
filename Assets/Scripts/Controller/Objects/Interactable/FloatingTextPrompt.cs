using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingTextPrompt : InGameButtonPrompt
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
        PositionText();
    }
    void Collect()
    {
        Destroy(this.gameObject);

    }
    void PositionText()
    {
        if (isOpen)
        {

            viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            viewportPosition.y += 0.1f;
            float x1 = Mathf.Max(0, viewportPosition.x - 0.45f);
            float x2 = Mathf.Min(1, viewportPosition.x + 0.45f);
            DynamicText.rectTransform.anchorMin = new Vector2(x1, viewportPosition.y - 0.2f);
            DynamicText.rectTransform.anchorMax = new Vector2(x2, viewportPosition.y + 0.2f);
            DynamicText.rectTransform.offsetMin = Vector2.zero;
            DynamicText.rectTransform.offsetMax = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DynamicText.text = Text;
        isOpen = true;
        Prompt();
    }
    void OnTriggerExit2D(Collider2D other)
    {
        DynamicText.text = string.Empty;
        isOpen = false;
        Deprompt();
    }
}