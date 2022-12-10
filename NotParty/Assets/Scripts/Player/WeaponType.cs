using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    public string weaponName;

    [SerializeField]
    private float efficiency;

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
            collision.GetComponent<WeaponVulnerable>().Hit();
            collision.GetComponent<WeaponVulnerable>().damaged += efficiency * GetComponentInParent<PlayerController>().GetEfficiencyMultiplier();
            Debug.Log(collision.GetComponent<WeaponVulnerable>().damaged);
        }
    }
}
