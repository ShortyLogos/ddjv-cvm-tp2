using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//@Source: https://www.youtube.com/watch?v=8oTYabhj248

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private ScenesHandler sceneHandler;
    [SerializeField]
    private TextMeshProUGUI textComponent;
    [SerializeField]
    private float textSpeed;
    [SerializeField]
    private string[] lines;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("PlayDialogue","True").Equals("True"))
        {
            gameObject.SetActive(true);
            textComponent.text = string.Empty;
            sceneHandler = GameObject.Find("ScenesHandler").GetComponent<ScenesHandler>();
            float delai = 0;
            if (sceneHandler != null)
            {
                delai = sceneHandler.GetIntroDuration();
            }
            StartDialogue(delai/2);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            } else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        } 
    }

    public void StartDialogue(float delai = 0)
    {
        index = 0;
        StartCoroutine(TypeLine(delai));
    }

    private IEnumerator TypeLine(float delai)
    {
        yield return new WaitForSeconds(delai);
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed); 
        }
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine(0));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
