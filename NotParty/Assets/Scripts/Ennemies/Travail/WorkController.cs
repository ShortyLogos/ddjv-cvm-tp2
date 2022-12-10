using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rig;

    private bool isMoving = false;
    private Vector3 direction;
    private Vector3 lookingToward;

    private Color originalColor;

    [SerializeField]
    private float speed = 4.0f;

    [SerializeField]
    private float maxSpeed = 7.0f;
    
    [SerializeField]
    private float minSpeed = 2.0f;

    [SerializeField]
    private float viewDistance = 0.5f;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject player;

    public bool dead;

    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        direction.z = 0.0f;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            lookingToward = (player.transform.position - transform.position).normalized;
            anim.SetFloat("moveX", lookingToward.x);

            if (!isMoving)
            {
                StartCoroutine(CMove());
            }
        }
    }

    private void FixedUpdate()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        if (!dead)
        {
            rig.AddForce(direction.normalized * speed, ForceMode2D.Force);
        }
    }

    public bool IsDead()
    {
        return dead;
    }

    IEnumerator CMove()
    {
        isMoving = true;
        bool valid = false;
        int tryToMove = 0;

        do
        {
            direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
            direction.Normalize();
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDistance, layerMask);

            if (hit.collider != null)
            {
                tryToMove++;
                if (tryToMove > 1000)
                {
                    direction = Vector3.zero;
                    valid = true;
                }
            }
            else
            {
                valid = true;
            }

        } while (!valid);

        yield return new WaitForSeconds(Random.Range(0.25f, 2.0f));
        direction = Vector3.zero;
        yield return new WaitForSeconds(Random.Range(0.25f, 2.0f));

        isMoving = false;
    }

    public IEnumerator CVainquished()
    {
        StopCoroutine(CMove());
        dead = true;
        anim.SetBool("dead", true);
        direction = Vector3.zero;
        
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);

        Debug.Log("Niveau complété.");
    }
}
