using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject loot;

    //void Start()
    //{
    //    loot = GameObject.Find("Cplusplus");
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<WeaponInventory>().addWeapon(loot);
        }
    }
}
