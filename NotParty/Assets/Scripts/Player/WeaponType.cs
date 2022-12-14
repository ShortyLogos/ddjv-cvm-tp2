using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    public string weaponName;

    [SerializeField]
    private float efficiency;

    void OnParticleCollision(GameObject collision)
    {
        if (collision.layer == LayerMask.NameToLayer("Work") || collision.layer == LayerMask.NameToLayer("Distractions"))
        {
            float damage = efficiency * GetComponentInParent<PlayerController>().GetEfficiencyMultiplier();
            collision.GetComponent<WeaponVulnerable>().Hit(damage);
        }
    }
}
