using UnityEngine;
using UnityEngine.UI;
using float_oat.Desktop90;
using System;
using System.Collections.Generic;

public class HighScoreHandler : MonoBehaviour
{
    // Score entering window after a defeat
    [SerializeField] private GameObject scoreUI;

    // Variables related to the high scores list
    [SerializeField] private int maxCount = 10;
    [SerializeField] private string filename = "highscore.json";
    private List<HighScoreEntry> highScoreList = new List<HighScoreEntry>();
    public List<HighScoreEntry> HighScoreList {
        get {
            return highScoreList;   
        }
    }

    // High score of the current game
    private int highscore;
    public int Highscore { get; }

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("score", 0);
        LoadHighscores();
    }

    // Called by the submit score button in the Score UI window
    public void SaveScore()
    {
        // Get the player's name
        InputField nameInput = scoreUI.GetComponentInChildren<InputField>();
        string playerName = nameInput.text;

        HighScoreEntry entry = new HighScoreEntry(playerName, highscore);
        
        // Check if the entry make it in the top high scores
        AddEntryIfPossible(entry);
        
        scoreUI.GetComponent<WindowController>().Close();
    }

    // Make sure the list stays under the maxcount and add only if the score
    // is better than the ones already there
    private void AddEntryIfPossible(HighScoreEntry entry)
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (i >= highScoreList.Count || entry.score > highScoreList[i].score)
            {
                // add new high score
                highScoreList.Insert(i, entry);

                while (highScoreList.Count > maxCount)
                {
                    highScoreList.RemoveAt(maxCount);
                }

                FileHandler.SaveToJSON<HighScoreEntry>(highScoreList, filename);

                break;
            }
        }
    }

    // Load the high scores list from a json file
    private void LoadHighscores()
    {
        highScoreList = FileHandler.ReadListFromJSON<HighScoreEntry>(filename);
    }

    // Increment the actual high score
    public void Victory()
    {
        highscore++;
        PlayerPrefs.SetInt("score", highscore);
    }

    // Prompt the user to enter its name and save his score
    public void Defeat()
    {
        PlayerPrefs.SetInt("score", 0);
        scoreUI.GetComponent<WindowController>().Open();
    } 
}

[Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;

    public HighScoreEntry (string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }

}
