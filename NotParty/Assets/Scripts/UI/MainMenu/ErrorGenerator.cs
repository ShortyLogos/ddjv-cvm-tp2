using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ErrorGenerator : MonoBehaviour
{
    [SerializeField] private Text zoneMessage;
    [SerializeField] private Text button;
    private readonly string[][] textList = new string[][]
    {
        new string[] { "What is love?", "Baby don't hurt me" },
        new string[] { "Windows was unable to detect your keyboard. Press F1 to retry or F2 to abort", "Cancel" },
        new string[] { "Windows 98 has detected a random error. This error occurs every once in a while. Please wait.", "OK" },
        new string[] { "Did you try turning it off and on again?", "OK" },
        new string[] { @"Windows found a virus

            explorer.exe", "Destroy files" },
        new string[] { "Error Code 418: User Error. It's not our fault!", "OK" },
        new string[] { "NO!!! BAD USER !!! You've been warned already. This file does not exist. Now you've made us catch this worthless exception and we're upset. Do not do this again.", "Sorry.." },
        new string[] { "Are you sure you want to delete the content of your hard drive?", "Proceed" }
    };

    private void Start()
    {
        GenerateErrorBox();
    }

    public void GenerateErrorBox()
    {
        int index = (int)Mathf.Floor(Random.Range(0.0f, (float)textList.Length));
        zoneMessage.text = textList[index][0];
        button.text = textList[index][1];
    }
}
