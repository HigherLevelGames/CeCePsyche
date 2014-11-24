using UnityEngine;
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
    float quickTimeLimit = 5f;
    float timer;
    bool win;
    GameObject currentEventObject;

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
        currentEventObject = Instantiate(GameController.Prompt) as GameObject;
        state = MiniGameState.Prompting;
    }

    void BeginMiniGame()
    {
        timer = gameTimeLimit;
        DestroyImmediate(currentEventObject);
        Game.SetActive(true);
        state = MiniGameState.Playing;
    }

    void Fail()
    {
        timer = quickTimeLimit;
        GameController.Reset();
        Game.SetActive(false);
        currentEventObject = Instantiate(GameController.Lose) as GameObject;
        state = MiniGameState.Losing;
    }

    void RePrompt()
    {
        timer = promptTimeLimit;
        DestroyImmediate(currentEventObject);
        currentEventObject = Instantiate(GameController.Hint) as GameObject;
        state = MiniGameState.Prompting;
    }

    void Success()
    {
        timer = quickTimeLimit;
        GameController.Reset();
        Game.SetActive(false);
        currentEventObject = Instantiate(GameController.Win) as GameObject;
        state = MiniGameState.Winning;
    }

    void Cleanup()
    {
        DestroyImmediate(currentEventObject);

        GameCamera.enabled = true;
        MiniGameCamera.enabled = false;
        Ceci.SetActive(true);
        state = MiniGameState.Uninitialized;
    }

}