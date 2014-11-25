using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiniGameController : InGameButtonPrompt
{
    enum MiniGameState
    {
        Uninitialized,
        Prompting,
        Playing,
        Winning, 
        Losing
    }
    MiniGameState state = MiniGameState.Uninitialized;
    public Camera MiniGameCamera;
    public Camera GameCamera;
    public GameObject Ceci;
    GameObject Game;
    PointNClickGame GameController;
    float gameTimeLimit = 30;
    float promptTimeLimit = 10;
    float quickTimeLimit = 4;
    float timer;
    bool win;
    Text[] currentEventText = new Text[2];

    void Awake()
    {
        Game = MiniGameCamera.gameObject.transform.GetChild(0).gameObject;
        GameController = Game.GetComponent<PointNClickGame>();
    }

    void Update()
    {
        switch (state)
        {
            case MiniGameState.Uninitialized:
                if (renderer.enabled)
                if (Input.GetKeyDown(KeyCode.E))
                    BeginPrompt();
                break;
            case MiniGameState.Prompting:
                timer -= Time.deltaTime;
                if (timer < 0 || Input.GetMouseButtonDown(0))
                    BeginMiniGame();
                break;
            case MiniGameState.Playing:
                timer -= Time.deltaTime;
                if (GameController.WinConditionMet)
                    Success();
                else if (timer < 0)
                    Fail();
                break;
            case MiniGameState.Losing:
                timer -= Time.deltaTime;
                if (timer < 0)
                    RePrompt();
                break;
            case MiniGameState.Winning:
                timer -= Time.deltaTime;
                if (timer < 0 || Input.GetMouseButtonDown(0))
                    Cleanup();
                break;

        }
    }

    void BeginPrompt()
    {
        timer = promptTimeLimit;
        GameCamera.enabled = false;
        Ceci.SetActive(false);
        MiniGameCamera.enabled = true;
        currentEventText[0] = Instantiate(GameController.Prompt) as Text;
        Debug.Log(currentEventText [0]);
        state = MiniGameState.Prompting;
    }

    void BeginMiniGame()
    {
        timer = gameTimeLimit;
        ClearEventObject();
        Game.SetActive(true);
        GameController.Activate();
        state = MiniGameState.Playing;
    }

    void Fail()
    {
        timer = quickTimeLimit;
        GameController.Reset();
        Game.SetActive(false);
        currentEventText[0] = Instantiate(GameController.Lose) as Text;
        state = MiniGameState.Losing;
    }

    void RePrompt()
    {
        timer = promptTimeLimit;
        ClearEventObject();
        currentEventText [0] = Instantiate(GameController.Prompt) as Text;
        currentEventText[1] = Instantiate(GameController.Hint) as Text;
        state = MiniGameState.Prompting;
    }

    void Success()
    {
        timer = quickTimeLimit;
        GameController.Reset();
        Game.SetActive(false);
        currentEventText[0] = Instantiate(GameController.Win) as Text;
        state = MiniGameState.Winning;
    }

    void Cleanup()
    {
        ClearEventObject();
        GameController.Cleanup();
        GameCamera.enabled = true;
        MiniGameCamera.enabled = false;
        Ceci.SetActive(true);
        state = MiniGameState.Uninitialized;
    }
    void ClearEventObject()
    {
        for (int i = 0; i < currentEventText.Length; i++)
            if (currentEventText[i])
                DestroyImmediate(currentEventText [i]);
    }
}
