using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//@Source: https://www.youtube.com/watch?v=2gPHkaPGbpI

public class Timer : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private GameObject gameHandler;

    public int Duration;

    private int remainingDuration;

    // Start is called before the first frame update
    void Start()
    {
        Being(Duration);
    }

    private void Being(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            uiText.text = $"{remainingDuration / 60:00} : {remainingDuration % 60:00}";
            uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        //End Time, if want do something
        Debug.Log("End");
        gameHandler.GetComponent<GameHandler>().Defeat();
    }
}
