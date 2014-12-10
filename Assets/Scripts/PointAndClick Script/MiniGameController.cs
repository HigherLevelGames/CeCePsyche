using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiniGameController : FloatingTextPrompt
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
    Text textPrompt;
    Text hint;

    void Awake()
    {
        Game = MiniGameCamera.gameObject;
        GameController = Game.GetComponent<PointNClickGame>();
        textPrompt = CanvasManager.data.PromptText;
        hint = CanvasManager.data.HintText;
        textPrompt.text = "";
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
        GameController.Reset();
        Ceci.SetActive(false);
        MiniGameCamera.enabled = true;
        textPrompt.text = GameController.Prompt;
        state = MiniGameState.Prompting;
    }

    void BeginMiniGame()
    {
        timer = gameTimeLimit;
        Game.SetActive(true);
        textPrompt.text = "";
        hint.text = "";
        GameController.Activate();
        state = MiniGameState.Playing;
    }

    void Fail()
    {
        timer = quickTimeLimit;
        Game.SetActive(false);
        textPrompt.text = GameController.Lose;
        state = MiniGameState.Losing;
    }

    void RePrompt()
    {
        timer = promptTimeLimit;
        GameController.Reset();
        textPrompt.text = GameController.Prompt;
        hint.text = GameController.Hint;
        state = MiniGameState.Prompting;
    }

    void Success()
    {
        timer = quickTimeLimit;
        Game.SetActive(false);
        textPrompt.text = GameController.Win;
        state = MiniGameState.Winning;
    }

    void Cleanup()
    {
        textPrompt.text = "";
        hint.text = "";
        GameController.Cleanup();
        GameCamera.enabled = true;
        MiniGameCamera.enabled = false;
        Ceci.SetActive(true);
        state = MiniGameState.Uninitialized;
    }
}
