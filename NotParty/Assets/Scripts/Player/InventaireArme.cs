using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventaireArme : MonoBehaviour
{
    public List<GameObject> liste = new List<GameObject>();
    public GameObject armeInitiale;

    // Start is called before the first frame update
    void Start()
    {
        if (armeInitiale != null)
        {
            ajoutArme(armeInitiale);
        }
    }

    public void ajoutArme(GameObject arme)
    {
        bool nouvelle = true;
        string nomArme = arme.GetComponent<ArmeLanguage>().nom;

        int i = 0;
        int index = 0;
        foreach (GameObject armePossedee in liste)
        {
            if (armePossedee.GetComponent<ArmeLanguage>().nom.Equals(nomArme) && nouvelle)
            {
                index = i;
                nouvelle = false;
            }

            i++;
        }

        if (nouvelle)
        {
            liste.Add(arme);
            this.gameObject.GetComponent<ArmeCourante>().NouvelleArme();
        }
        else
        {
            this.gameObject.GetComponent<ArmeCourante>().ChangerArme(index);
        }
    }
}
