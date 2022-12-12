using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pagination : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pages;
    
    private int index = 0;
    private int nbrPages;
    
    [SerializeField]
    private GameObject btnLast;
    
    [SerializeField]
    private GameObject btnNext;

    private void Start()
    {
        nbrPages = pages.Length;
    }

    public void NextPage()
    {
        if (index<nbrPages)
        {
            pages[index].SetActive(false);
            index++;
            pages[index].SetActive(true);
        }
        UpdateBtn();
    }

    public void LastPage()
    {
        if (index > 0)
        {
            pages[index].SetActive(false);
            index--;
            pages[index].SetActive(true);
        }
        UpdateBtn();
    }

    private void UpdateBtn()
    {
        if (index == 0) {
            btnLast.SetActive(false);
        } else {
            btnLast.SetActive(true);
        }

        if (index == nbrPages - 1) {
            btnNext.SetActive(false);
        } else {
            btnNext.SetActive(true);
        }
    }
}
