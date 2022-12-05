using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// @Source: https://github.com/MichelleFuchs/PointCounter_Unity/tree/master/PointCount_E4

[RequireComponent(typeof(HighScoreHandler))]
public class HighScoreListUI : MonoBehaviour
{
    [SerializeField] private GameObject highscoreUIElementPrefab;
    [SerializeField] private Transform elementWrapper;
    
    List<GameObject> uiElements = new List<GameObject>();
    List<HighScoreEntry> scoreList = new List<HighScoreEntry>(); 

    private void Start()
    {
        scoreList = GetComponent<HighScoreHandler>().HighScoreList;
        UpdateUI();
    }

    private void UpdateUI() {
        if (scoreList.Count == 0)
        {
            scoreList.Add(new HighScoreEntry("AAA", 2));
            scoreList.Add(new HighScoreEntry("Karen66", 4));
            scoreList.Add(new HighScoreEntry("NoobMaster", 1));
            scoreList.Add(new HighScoreEntry("Couch_Potato", 48));
        }
        for (int i = 0; i< scoreList.Count; i++)
        {
            HighScoreEntry el = scoreList[i];

            if (el.score > 0)
            {
                if (i >= uiElements.Count)
                {
                    // instantiate new entry
                    var inst = Instantiate(highscoreUIElementPrefab, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent(elementWrapper, false);

                    uiElements.Add(inst);
                }
                Text text = uiElements[i].GetComponentInChildren<Text>();
                text.text = el.playerName + " survived " + el.score.ToString() + " end of semesters";
            }
        }
    }
}
