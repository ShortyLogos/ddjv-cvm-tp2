using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paginations : MonoBehaviour
{
    [SerializeField]
    private GameObject btnNext;
    [SerializeField]
    private GameObject btnLast;
    [SerializeField]
    private GameObject[] pages;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
              
    }

    public void NextPage()
    {
        pages[index].SetActive(false);
        index++;
        pages[index].SetActive(true);
        UpdateUI();
    }

    public void LastPage()
    {
        pages[index].SetActive(false);
        index--;
        pages[index].SetActive(true);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (index == 0)
        {
            btnLast.SetActive(false);
        } else
        {
            btnLast.SetActive(true);
        }
        if (index == pages.Length - 1)
        {
            btnNext.SetActive(false);
        } else
        {
            btnNext.SetActive(true);
        }
    }

}
