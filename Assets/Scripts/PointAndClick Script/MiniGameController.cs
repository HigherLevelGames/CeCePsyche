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
    public Canvas MasterCanvas;
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
    Text text;
    Text hint;

    void Awake()
    {
        Game = MiniGameCamera.gameObject.transform.GetChild(0).gameObject;
        GameController = Game.GetComponent<PointNClickGame>();
        Text[] texts = MasterCanvas.GetComponentsInChildren<Text>();
        Debug.Log(texts.Length);
        text = texts [0];
        hint = texts [1];
        text.text = "";
        hint.text = "";
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
        text.text = GameController.Prompt;
        state = MiniGameState.Prompting;
    }

    void BeginMiniGame()
    {
        timer = gameTimeLimit;
        Game.SetActive(true);
        text.text = "";
        hint.text = "";
        GameController.Activate();
        state = MiniGameState.Playing;
    }

    void Fail()
    {
        timer = quickTimeLimit;
        GameController.Reset();
        Game.SetActive(false);
        text.text = GameController.Lose;
        state = MiniGameState.Losing;
    }

    void RePrompt()
    {
        timer = promptTimeLimit;
        text.text = GameController.Prompt;
        hint.text = GameController.Hint;
        state = MiniGameState.Prompting;
    }

    void Success()
    {
        timer = quickTimeLimit;
        GameController.Reset();
        Game.SetActive(false);
        text.text = GameController.Win;
        state = MiniGameState.Winning;
    }

    void Cleanup()
    {
        text.text = "";
        hint.text = "";
        GameController.Cleanup();
        GameCamera.enabled = true;
        MiniGameCamera.enabled = false;
        Ceci.SetActive(true);
        state = MiniGameState.Uninitialized;
    }
}
