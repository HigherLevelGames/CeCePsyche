using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BookButtonPrompt : InGameButtonPrompt
{
    public Text bookText;
    public Image bookImage;
    public string Title;
    public string[] Pages;
    int page;

    void Start()
    {
        bookText.text = string.Empty;
        bookImage.enabled = false;
    }

    void Update()
    {
        if (renderer.enabled) 
        if (Input.GetKeyDown(KeyCode.E))
            OpenBook();
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
