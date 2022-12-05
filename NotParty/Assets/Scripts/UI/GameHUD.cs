using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highscoreHUD;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetHighScore(int score)
    {
        highscoreHUD.text = "Score: "+score.ToString();
    }
    
}
