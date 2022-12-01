using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ref : https://www.youtube.com/watch?v=7c68z05vaX4

public class Arme : MonoBehaviour
{
    public GameObject arme;

    // Start is called before the first frame update
    void Start()
    {
        arme.GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            arme.GetComponent<ParticleSystem>().Play();
        }

        if (Input.GetMouseButtonUp(0))
        {
            arme.GetComponent<ParticleSystem>().Stop();
        }
    }
}
