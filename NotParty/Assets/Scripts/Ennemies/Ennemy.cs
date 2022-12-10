using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rig;

    [SerializeField]
    private float chaseSpeed = 1.0f, patrolSpeed = 0.5f;
    [SerializeField]
    private Vector3 movementDirection;
    private Vector3 visionDirection;

    [SerializeField]
    private string targetName;
    private GameObject target;
    [SerializeField]
    private LayerMask maskLayers;

    [SerializeField]
    private float viewDistance = 10.0f;

    private enum StateTree
    {
        eInactive = 1,
        ePatrol = 2,
        eChase = 4
    };
    private StateTree state;

    // Determine of its actually moving
    private bool IsMobile()
    {
        return (state & (StateTree.ePatrol | StateTree.eChase)) != 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        movementDirection.z = 0.0f;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.Find(targetName);
    }

    // Update is called once per frame
    void Update()
    {
        visionDirection = (target.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, visionDirection, viewDistance, maskLayers);

        float distance_destination = viewDistance;
        StateTree oldState = state;
        state = StateTree.ePatrol;

        if (hit.collider != null)
        {
            distance_destination = hit.distance;
            if (hit.collider.gameObject.layer == target.layer)
            {
                state = StateTree.eChase;
                movementDirection = visionDirection;
            }
        }

        Vector3 delta = visionDirection * distance_destination;
        Debug.DrawRay(transform.position, delta, Color.cyan);

        if (oldState != state)
        {
            if (state == StateTree.ePatrol)
            {
                StartCoroutine(CPatrouille());
            }
            else if (state == StateTree.eChase)
            {
                StopCoroutine(CPatrouille());
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsMobile())
        {
            anim.SetFloat("moveX", movementDirection.x);
            anim.SetFloat("moveY", movementDirection.y);
            anim.SetFloat("Speed", movementDirection.sqrMagnitude);
            float speed = state == StateTree.eChase ? chaseSpeed : patrolSpeed;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, movementDirection, viewDistance / 5, maskLayers);
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                movementDirection = Vector3.zero;
            }
            rig.velocity = movementDirection * speed;
            if (movementDirection.sqrMagnitude > 0)
            {
                anim.SetFloat("lastMoveX", movementDirection.x);
                anim.SetFloat("lastMoveY", movementDirection.y);
            }
        }
        else
        {
            rig.velocity = Vector2.zero;
            anim.SetFloat("Speed", 0.0f);
        }
    }

    IEnumerator CPatrouille()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            bool valid = false;
            int attempts = 0;
            do
            {
                movementDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
                movementDirection.Normalize();
                RaycastHit2D frappe = Physics2D.Raycast(transform.position, movementDirection, viewDistance / 2, maskLayers);
                if (frappe.collider != null)
                {
                    attempts++;
                    if (attempts > 30)
                    {
                        movementDirection = Vector3.zero;
                        valid = true;
                    }
                }
                else
                {
                    valid = true;
                }
            } while (!valid);
            yield return new WaitForSeconds(Random.Range(1.5f, 3.0f));
            movementDirection = Vector3.zero;
            yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
        }
    }

}
