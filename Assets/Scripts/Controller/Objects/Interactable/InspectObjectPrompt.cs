using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InspectObjectPrompt : InGameButtonPrompt
{

    public string Title;
    public string[] AdditionalInformation;
    public Sprite sprite;
    Text objectText;
    Image objectImage;
    int page;

    void Start()
    {
        objectImage = CanvasManager.data.ObjectImage;
        objectText = CanvasManager.data.ObjectText;
        objectText.text = string.Empty;
        objectImage.enabled = false;
    }

    void Update()
    {
        if (isOpen)
        {
            if (RebindableInput.GetKeyDown("Interact"))
            {
                InteractObject();
                return;
            }
            PositionPrompt();
        }
    }

    void InteractObject()
    {
        if (objectText.text == string.Empty)
        {
            objectImage.enabled = true;
            objectText.text = Title;
            objectImage.sprite = sprite;
        } else
        {
            if (page < AdditionalInformation.Length)
            {
                objectText.text = AdditionalInformation [page];
                page++;
            } else
            {
                objectText.text = string.Empty;
                objectImage.enabled = false;
                page = 0;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        objectText.text = string.Empty;
        objectImage.enabled = false;
        page = 0;
        Deprompt();
    }
}
