using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ScenesHandler: MonoBehaviour
{
    private GameObject fadeScreen;
    private List<string> levelsList = new List<string>();
    private float previousTimeScale = 1;

    private static bool isPaused;
    public bool IsPaused
    {
        get
        {
            return isPaused;
        }
    }

    public float GetIntroDuration()
    {
        return GetFadeScreen().GetComponent<FadeScreen>().GetFadeInDuration();
    }

    public float GetOutroDuration()
    {
        return GetFadeScreen().GetComponent<FadeScreen>().GetFadeOutDuration();
    }

    private GameObject GetFadeScreen()
    {
        if (fadeScreen == null)
        {
            fadeScreen = GameObject.FindWithTag("FadeScreen");
        }
        if (fadeScreen == null && Application.isEditor)
        {
            Debug.LogError("No objet with the tag FadeScreen have been found.");
        }
        return fadeScreen;
    }

    private void Start()
    {
        if (fadeScreen == null)
        {
            GetFadeScreen();
        }
        if (SceneManager.GetActiveScene().name == "BootScreen")
        {
            NextScene(pause: false, delai: fadeScreen.GetComponent<FadeScreen>().GetTotalDuration());
        } else if (SceneManager.GetActiveScene().name == "ShutDown")
        {
            float delai = fadeScreen.GetComponent<FadeScreen>().GetTotalDuration();
            StartCoroutine(CQuitApp(delai));
        }
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene current)
    {
        if (isPaused)
        {
            TogglePause();
        }
    }

    public void Quit()
    {
        PlayScene("Scenes/ShutDown", false, 3.0f, true);
    }

    public void MainMenu()
    {
        PlayScene("Scenes/MainMenu", false, fadeOut:true);
    }

    public void PlayScene(string scene, bool pause = false, float delai = 0.0f, bool fadeOut = false)
    {
        StartCoroutine(CPlayScene(scene, delai, pause, fadeOut));
    }

    public void NextScene(bool pause = false, float delai = 0.0f, bool fadeOut = false)
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(CPlayScene(index, delai, pause, fadeOut));
    }

    public void LastScene(bool pause = false, float delai = 0.0f, bool fadeOut = false)
    {
        int index = SceneManager.GetActiveScene().buildIndex -1;
        StartCoroutine(CPlayScene(index, delai, pause, fadeOut));
    }

    public void RestartScene(bool pause = false, float delai = 0.0f, bool fadeOut = false)
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(CPlayScene(index, delai, pause, fadeOut));
    }

    public void PlayRandomLevel()
    {
        string level = GetRandomLevel();
        if (level != "")
        {
            PlayScene(level, fadeOut: true);
        }
    }

    public string GetRandomLevel()
    {
        BuildLevelList();
        string newScene = "";
        if (levelsList.Count>0)
        {
            string currentScene = "Scenes/Levels/" + SceneManager.GetActiveScene().name;
            do
            {
                int randomIndex = (int)Mathf.Floor(Random.Range(0.0f, (float)levelsList.Count));
                newScene = levelsList[randomIndex];
            } while (newScene == currentScene);
        } else
        {
            Debug.Log("Sorry but you'll have to pay for the DLC to unlock the levels of the game.");
        }
        return newScene;
    }

    private void BuildLevelList()
    {
        DirectoryInfo dir = new DirectoryInfo("Assets/Scenes/Levels/");
        string extension = ".unity";
        FileInfo[] filesList = dir.GetFiles();
        foreach (FileInfo file in filesList) {
            string fileName = file.Name;
            if (fileName.EndsWith(extension))
            {
                string cleanName = Path.GetFileNameWithoutExtension(fileName);
                levelsList.Add("Scenes/Levels/" + cleanName);
            }
        }
    }

    private float ActivateFadeOut()
    {
        return GetFadeScreen().GetComponent<FadeScreen>().FadeOut();
    }

    private IEnumerator CPlayScene(string name, float delai, bool pause = false, bool fadeOut = false)
    {
        if (fadeOut) delai += ActivateFadeOut();
        if (pause) TogglePause();
        yield return StartCoroutine(Utility.WaitForRealSeconds(delai));
        SceneManager.LoadScene(name);
    }

    private IEnumerator CPlayScene(int index, float delai, bool pause = false, bool fadeOut = false)
    {
        if (fadeOut) delai += ActivateFadeOut();
        if (pause) TogglePause();
        yield return StartCoroutine(Utility.WaitForRealSeconds(delai));
        SceneManager.LoadScene(index);
    }

    private IEnumerator CQuitApp(float delai = 0.0f)
    {
        yield return StartCoroutine(Utility.WaitForRealSeconds(delai));
        if (Application.isEditor)
        {
            Debug.Log("Screw you guys, i'm going home! -Eric Cartman");
        }
        Application.Quit();
    }

    public void TogglePause()
    {
        // Source : https://www.youtube.com/watch?v=ROwsdftEGF0
        
        if (!isPaused)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        else if (isPaused)
        {
            Time.timeScale = previousTimeScale;
            AudioListener.pause = false;
        }

        isPaused = !isPaused;
    }


}
