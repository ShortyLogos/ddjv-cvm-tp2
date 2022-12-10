using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    public string weaponName;

    [SerializeField]
    private int efficiency;

    [SerializeField]
    private float cooldown;

    private ParticleSystem particules;

    void Start()
    {
        particules = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject collision)
    {
        if (collision.tag == "Work" || collision.tag == "Distraction")
        {
            collision.GetComponent<VulnerableArme>().Hit();
            collision.GetComponent<VulnerableArme>().degatAccumule += efficiency;
            Debug.Log(collision.GetComponent<VulnerableArme>().degatAccumule);
        }
    }
}
