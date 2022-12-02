using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ref : https://www.youtube.com/watch?v=7c68z05vaX4

public class ArmeCourante : MonoBehaviour
{
    public GameObject arme;

    private int indexArme = 0;
    private InventaireArme inventaire;

    // Start is called before the first frame update
    void Start()
    {
        arme.GetComponent<ParticleSystem>().Stop();
        inventaire = GetComponent<InventaireArme>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Je cycle vers la droite");

            indexArme++;
            indexArme = indexArme % inventaire.liste.Count;
            arme = inventaire.liste[indexArme];
            Debug.Log(indexArme);
        }

        if (Input.GetMouseButtonDown(0))
        {
            arme.GetComponent<ParticleSystem>().Play();
        }

        if (Input.GetMouseButtonUp(0))
        {
            arme.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void SetArme(GameObject nouvelleArme)
    {
        arme = nouvelleArme;
    }
}
