using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootArme : MonoBehaviour
{
    [SerializeField]
    private GameObject loot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Joueur"))
        {
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<InventaireArme>().ajoutArme(loot);
        }
    }
}
