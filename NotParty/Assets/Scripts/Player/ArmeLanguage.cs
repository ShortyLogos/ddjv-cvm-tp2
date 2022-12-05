using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmeLanguage : MonoBehaviour
{
    public string nom;

    [SerializeField]
    private int efficacite;

    [SerializeField]
    private float cooldown;

    private ParticleSystem particules;

    void Start()
    {
        particules = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject collision)
    {
        if (collision.tag == "Travail" || collision.tag == "Distraction")
        {
            collision.GetComponent<VulnerableArme>().degatAccumule += efficacite;
            Debug.Log(collision.GetComponent<VulnerableArme>().degatAccumule);
        }
    }
}
