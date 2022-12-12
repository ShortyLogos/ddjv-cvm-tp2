using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorkController : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rig;

    protected bool isMoving = false;
    protected Vector3 direction;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected float maxSpeed;

    [SerializeField]
    protected float minSpeed;

    [SerializeField]
    protected float viewDistance;

    [SerializeField]
    protected float deathAnimDuration;

    [SerializeField]
    protected float specialAbilityDuration;

    [SerializeField]
    protected LayerMask layerMask;

    [SerializeField]
    protected GameObject player;

    protected bool dead = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Standard Stuff
        direction.z = 0.0f;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!dead)
        {
            Vector3 lookingToward = (player.transform.position - transform.position);
            anim.SetFloat("moveX", lookingToward.x);
            if (!isMoving)
            {
                StartCoroutine(CMove());
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!dead)
        {
            speed = Random.Range(minSpeed, maxSpeed);
            rig.AddForce(direction.normalized * speed, ForceMode2D.Force);
        }
    }

    protected virtual IEnumerator CMove()
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

    public virtual IEnumerator CVainquished()
    {
        if (!dead)
        {
            dead = true;
            Destroy(GetComponent<Rigidbody2D>()); //Immobiliser l'objet
            anim.SetBool("dead", dead);
            yield return new WaitForSeconds(deathAnimDuration);
            Destroy(this.gameObject);
        }
    }

    public abstract float ActivateAbility();

}
