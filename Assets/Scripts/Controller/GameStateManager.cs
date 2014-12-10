using UnityEngine;
using System.Collections;

#region Related Enumerations
public enum GameState
{
    Playing,
    InMenu,
    Exiting
}
#endregion
public class GameStateManager : MonoBehaviour
{
    #region Singleton Initialization
    public static GameStateManager data;

    void Initialize()
    {

    }

    void Awake()
    {
        if (data == null)
        {
            DontDestroyOnLoad(this);
            Initialize();
            data = this;
        } 
    }
    #endregion
    public GameState state;
    public int levelnum;
    public string levelname;

    void Update()
    {
        switch (state)
        {
            case GameState.Playing:
                GameUpdate();
                break;
        }
    }

    void GameUpdate()
    {
    }

    void GoToNextArea()
    {
        if (levelnum > -1)
        {
            Application.LoadLevel(levelnum);
            return;
        } else if (levelname != string.Empty)
        {
            Application.LoadLevel(levelname);
            return;
        }
        Debug.Log("The level was unable to be loaded due to impossible load name or number");
    }
    public void Load()
    {
        GoToNextArea();
    }
}
