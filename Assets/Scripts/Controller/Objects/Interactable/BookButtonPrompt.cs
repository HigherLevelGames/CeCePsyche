using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BookButtonPrompt : InGameButtonPrompt
{

    public string Title;
    public string[] Pages;
    Text bookText;
    Image bookImage;
    int page;

    void Start()
    {
        bookImage = CanvasManager.data.BookImage;
        bookText = CanvasManager.data.BookText;
        bookText.text = string.Empty;
        bookImage.enabled = false;
    }

    void Update()
    {
        if (isOpen)
        {
            if (RebindableInput.GetKeyDown("Interact"))
            {
                OpenBook();
                return;
            }
            PositionPrompt();
        }
    }

    void OpenBook()
    {
        if (bookText.text == string.Empty)
        {
            bookImage.enabled = true;
            bookText.text = Title;
        } else
        {
            if (page < Pages.Length)
            {
                bookText.text = Pages [page];
                page++;
            } else
            {
                bookText.text = string.Empty;
                bookImage.enabled = false;
                page = 0;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        bookText.text = string.Empty;
        bookImage.enabled = false;
        page = 0;
        Deprompt();
    }
}
