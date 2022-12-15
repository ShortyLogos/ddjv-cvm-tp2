using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//@Source: https://www.youtube.com/watch?v=YOaYQrN1oYQ

public class UserSettings : MonoBehaviour
{
    [SerializeField] 
    private AudioMixer mixer;
    [SerializeField] 
    private Dropdown resolutionDropdown;
    [SerializeField]
    private GameObject toggleDialogues;

    Resolution[] resolutions;

    private void Start()
    {
        if (resolutionDropdown != null) BuildResolutionList();
        if (toggleDialogues != null) {
            bool isActive = PlayerPrefs.GetString("PlayDialogue", "True").Equals("True");
            toggleDialogues.GetComponent<Toggle>().isOn = isActive;
        }
        mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("volume", 0f));
    }

    private void BuildResolutionList()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume (float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        mixer.SetFloat("MusicVolume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetDialogues (bool isActive)
    {
        PlayerPrefs.SetString("PlayDialogue", isActive.ToString());
    }
}
