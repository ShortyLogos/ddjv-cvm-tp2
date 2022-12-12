using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootWeapon : MonoBehaviour
{
    [SerializeField]
    private string weaponName;
    private GameObject loot;

    void Start()
    {
        loot = GameObject.Find("Player/WeaponDirection/"+weaponName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<WeaponInventory>().addWeapon(loot);
            Destroy(this.gameObject);
        }
    }
}
