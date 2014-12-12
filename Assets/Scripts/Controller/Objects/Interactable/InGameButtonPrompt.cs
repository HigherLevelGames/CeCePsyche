using UnityEngine;
using System.Collections;

public class InGameButtonPrompt : MonoBehaviour
{
    public AudioClip prompt;
    public AudioClip deprompt;
    public AudioClip activate;
    protected bool isOpen;

    void OnTriggerEnter2D(Collider2D other)
    {
        Prompt();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Deprompt();
    }

    public void Prompt()
    {
        isOpen = true;
        CanvasManager.data.KeyImage.gameObject.SetActive(true);
        CanvasManager.data.KeyText.text = RebindableInput.GetKeyFromBinding("Interact").ToString();
        audio.PlayOneShot(prompt);
    }

    public void Activate()
    {
        isOpen = false;
        CanvasManager.data.KeyImage.gameObject.SetActive(false);
        audio.PlayOneShot(activate);
    }

    public void Deactivate()
    {
        isOpen = true;
        CanvasManager.data.KeyImage.gameObject.SetActive(true);
        audio.PlayOneShot(activate);
    }

    public void Deprompt()
    {
        isOpen = false;
        CanvasManager.data.KeyImage.gameObject.SetActive(false);
        audio.PlayOneShot(deprompt);
    }

    protected void PositionPrompt()
    {
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        CanvasManager.data.KeyImage.rectTransform.anchorMin = viewportPosition;
        CanvasManager.data.KeyImage.rectTransform.anchorMax = viewportPosition;
        viewportPosition.y += 0.1f;
        viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0.1f, 0.9f);
        float x1 = viewportPosition.x - 0.45f;
        float x2 = viewportPosition.x + 0.45f;
        CanvasManager.data.DynamicText.rectTransform.anchorMin = new Vector2(x1, viewportPosition.y - 0.2f);
        CanvasManager.data.DynamicText.rectTransform.anchorMax = new Vector2(x2, viewportPosition.y + 0.2f);
        CanvasManager.data.DynamicText.rectTransform.offsetMin = Vector2.zero;
        CanvasManager.data.DynamicText.rectTransform.offsetMax = Vector2.zero;
    }
}
