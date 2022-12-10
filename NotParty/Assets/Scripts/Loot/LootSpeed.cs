using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpeed : MonoBehaviour
{
    [SerializeField]
    private float speedBoost = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<PlayerController>().AddSpeed(speedBoost);
        }
    }
}
