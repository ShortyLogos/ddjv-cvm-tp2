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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ajoutArme(GameObject arme)
    {
        liste.Add(arme);
    }
}
