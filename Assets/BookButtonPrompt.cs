using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BookButtonPrompt : InGameButtonPrompt
{
    public string Title;
    public string[] Pages;
    public Text book;
    int page;
    void Start()
    {
        book.text = string.Empty;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenBook();
        }
    }

    void OpenBook()
    {
        if (book.text == string.Empty)
        {
            book.text = Title;
        } else
        {
            if (page < Pages.Length)
            {
                book.text = Pages [page];
                page++;
            }
            else 
            {
                book.text = string.Empty;
                page = 0;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        book.text = string.Empty;
        page = 0;
        Disable();
    }
}
