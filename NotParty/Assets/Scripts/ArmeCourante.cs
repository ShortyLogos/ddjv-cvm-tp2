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
        //arme.GetComponent<ParticleSystem>().Stop();
        inventaire = GetComponent<InventaireArme>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && arme != null)
        {
            arme.GetComponent<ParticleSystem>().Play();
        }

        if (Input.GetMouseButtonUp(0) && arme != null)
        {
            arme.GetComponent<ParticleSystem>().Stop();
        }

        bool cycleGauche = Input.GetKeyDown(KeyCode.Q);
        bool cycleDroite = Input.GetKeyDown(KeyCode.E);
        if (cycleDroite || cycleGauche)
        {
            if (arme != null)
            {
                arme.GetComponent<ParticleSystem>().Stop();
            }

            if (inventaire.liste.Count > 0)
            {
                if (cycleDroite)
                {
                    indexArme++;
                }
                else
                {
                    indexArme--;
                }

                indexArme = indexArme % inventaire.liste.Count;

                arme = inventaire.liste[Mathf.Abs(indexArme)];
                Debug.Log(indexArme);
            }

            if (Input.GetMouseButton(0) && arme != null)
            {
                arme.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    public void SetArme(GameObject nouvelleArme)
    {
        arme = nouvelleArme;
    }
}
