using UnityEngine;
using System.Collections;

#region Related Enumerations
public enum GameState
{
    Loading,
    Playing,
    InMenu,
    Exiting
}
#endregion
public class GameStateManager : MonoBehaviour
{
    #region Singleton Initialization
    public static GameStateManager data;
    void Awake()
    {
        if (data == null)
            data = this;
    }
    #endregion
    public GameObject Ceci;
    public GameState state;
    public int levelnum;
    public string levelname;

    void Update()
    {
        switch (state)
        {
            case GameState.Loading:
                break;
            case GameState.Playing:
                break;
        }
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
