using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HighScoreHandler))]
[RequireComponent(typeof(ScenesHandler))]
public class GameHandler : MonoBehaviour
{
    [SerializeField] private HighScoreHandler highscoreHandler;
    [SerializeField] private ScenesHandler sceneHandler;
    [SerializeField] private GameObject gameHUD;

    public bool IsPaused {
        get {
            return sceneHandler.IsPaused;
        } 
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        if (sceneHandler.IsPaused)
        {
            sceneHandler.TogglePause();
        }
        gameHUD.GetComponent<GameHUD>().SetHighScore(+highscoreHandler.Highscore);
    }

    // Open a prompt to ask for the user's name to keep its highscore
    public void Defeat()
    {
        sceneHandler.TogglePause();
        highscoreHandler.Defeat();
    }

    // After saving the score, the game goes back to the main menu.
    public void SaveScore() 
    {
        highscoreHandler.SaveScore();
        ReturnMenu();
    }

    public void ReturnMenu()
    {
        sceneHandler.Quit();
    }

    // Increment the score and load a new random level
    public void Victory()
    {
        highscoreHandler.Victory();
        sceneHandler.PlayRandomLevel();
    }
}
