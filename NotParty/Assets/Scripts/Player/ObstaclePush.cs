using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePush : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Rigidbody2D rig = col.gameObject.GetComponent<Rigidbody2D>();
            if (rig != null)
            {
                rig.mass = 1;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Rigidbody2D rig = col.gameObject.GetComponent<Rigidbody2D>();
            if (rig != null)
            {
                rig.mass = 1000;
            }
        }
    }
}
