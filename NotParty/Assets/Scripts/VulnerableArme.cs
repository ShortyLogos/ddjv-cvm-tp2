using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableArme : MonoBehaviour
{
    [SerializeField]
    private string nom;

    public int degatAccumule; // ce � quoi l'on va comparer la progression
    public int vieMax; // ce � quoi l'on va comparer la progression

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (degatAccumule >= vieMax)
        {
            Debug.Log("Niveau compl�t�.");
        }
    }
}
