using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmeLanguage : MonoBehaviour
{
    [SerializeField]
    private string nom;

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
            // Ici, on va r�cup�rer le gameObject collision et modifier sa variable de progression (Travail) ou vie (Distraction)
            // selon l'efficacit� de l'arme s�lectionn�e
        collision.GetComponent<VulnerableArme>().degatAccumule += efficacite;
        Debug.Log(collision.GetComponent<VulnerableArme>().degatAccumule);
    }
}
