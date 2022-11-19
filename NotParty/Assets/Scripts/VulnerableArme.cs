using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableArme : MonoBehaviour
{
    public int progressionTravail = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        progressionTravail += 1;
        Debug.Log(progressionTravail);
    }
}
