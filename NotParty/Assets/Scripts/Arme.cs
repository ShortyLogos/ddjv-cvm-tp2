using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ref : https://www.youtube.com/watch?v=7c68z05vaX4

public class Arme : MonoBehaviour
{
    public ParticleSystem arme;

    // Start is called before the first frame update
    void Start()
    {
        arme = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            arme.Play();
        }

        if (Input.GetKeyUp("space"))
        {
            arme.Stop();
        }
    }
}
