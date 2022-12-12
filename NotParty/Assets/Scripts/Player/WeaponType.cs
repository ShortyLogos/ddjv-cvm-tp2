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
        if (collision.CompareTag("Work") || collision.CompareTag("Distraction"))
        {
            float damage = efficiency * GetComponentInParent<PlayerController>().GetEfficiencyMultiplier();
            collision.GetComponent<WeaponVulnerable>().Hit(damage);
        }
    }
}
