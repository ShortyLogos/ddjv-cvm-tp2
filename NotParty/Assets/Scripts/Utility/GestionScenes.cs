using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionScenes: MonoBehaviour
{
    private GameObject fadeScreen;
    
    private float previousTimeScale = 1;
    private static bool isPaused;
    public bool IsPaused
    {
        get => isPaused;
    }

    private void Start()
    {
        fadeScreen = GameObject.FindWithTag("FadeScreen");
        if (SceneManager.GetActiveScene().name == "BootScreen")
        {
            ProchaineScene(pause: false, delai: fadeScreen.GetComponent<FadeScreen>().GetTotalDuration());
        }
    }

    public void ProchaineScene(bool pause = false, float delai = 0.0f, bool fadeOut = false)
    {
        if (fadeOut)
        {
            delai += ActivateFadeOut();
        }
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(CJouerScene(index, delai, pause));
    }

    public void PrecedenteScene(bool pause = false, float delai = 0.0f, bool fadeOut = false)
    {
        if (fadeOut)
        {
            delai += ActivateFadeOut();
        }
        int index = SceneManager.GetActiveScene().buildIndex -1;
        StartCoroutine(CJouerScene(index, delai, pause));
    }

    public void RecommencerScene(bool pause = false, float delai = 0.0f, bool fadeOut = false)
    {
        if (fadeOut)
        {
            delai += ActivateFadeOut();
        }
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(CJouerScene(index, delai, pause));
    }

    private float ActivateFadeOut()
    {
        float delai = 0.0f;
        if (fadeScreen == null)
        {
            fadeScreen = GameObject.FindWithTag("FadeScreen");
        }
        if(fadeScreen != null)
        {
            delai = fadeScreen.GetComponent<FadeScreen>().FadeOut();
        } else
        {
            Debug.LogError("Aucun objet avec le Tag FadeScreen n'a été trouvé.");
        }
        return delai;
    }

    private IEnumerator CJouerScene(int index, float delai, bool pause = false)
    {
        if(pause)
        {
            TogglePause();
        }
        yield return StartCoroutine(Utility.WaitForRealSeconds(delai));
        SceneManager.LoadScene(index);
    }

    public void TogglePause()
    {
        // Source : https://www.youtube.com/watch?v=ROwsdftEGF0
        
        if (Time.timeScale > 0)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.pause = true;
            isPaused = true;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = previousTimeScale;
            AudioListener.pause = false;
        }
    }


}
