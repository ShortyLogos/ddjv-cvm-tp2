using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoueurControleur : MonoBehaviour
{
    private Rigidbody2D rig;
    private SpriteRenderer rendu;
    private Animator anim;
    private TrailRenderer dashTrail;
    private bool vivant = true;
    private bool startedMoving = false;
    private bool invincible;
    private bool dashing;
    private bool peutDasher = true;

    [SerializeField]
    private Vector3 mouvement = new Vector3(0.0f, 0.0f, 0.0f);

    [SerializeField]
    private float dashSpeed = 8.0f;

    [SerializeField]
    private float dashDuration = 0.5f;

    [SerializeField]
    private float dashCooldown = 1.35f;

    [SerializeField]
    private float speed;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rendu = GetComponent<SpriteRenderer>();
        dashTrail = GetComponent<TrailRenderer>();
        //sceneManager = GameObject.FindWithTag("SceneManager");

        invincible = false;
        dashing = false;
        mouvement.z = 0.0f;
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (Mathf.Abs(inputX) > 0.01f || Mathf.Abs(inputY) > 0.01f)
        {
            startedMoving = true;
        }

        if (vivant && startedMoving)
        {
            mouvement.x = inputX;
            mouvement.y = inputY;
        }

        anim.SetFloat("mouseX", mousePos.x);
        anim.SetFloat("mouseY", mousePos.y);

        float speed = 0.0f;
        if (startedMoving)
        {
            speed = mouvement.sqrMagnitude;
        }

        anim.SetFloat("speed", speed);

        if (Input.GetKeyDown(KeyCode.Space) && !dashing && peutDasher)
        {
            StartCoroutine(CDash());
        }
    }

    void FixedUpdate()
    {
        if (startedMoving)
        {
            if (!dashing)
            {
                rig.velocity = mouvement.normalized * speed;
            }
            else
            {
                rig.AddForce(mouvement.normalized * dashSpeed, ForceMode2D.Force);
            }
        }
    }

    IEnumerator CDash()
    {
        dashing = true;
        peutDasher = false;
        dashTrail.emitting = true;
        anim.speed = 1.75f;
        yield return new WaitForSeconds(dashDuration);

        dashing = false;
        dashTrail.emitting = false;
        anim.speed = 1.0f;
        yield return new WaitForSeconds(dashCooldown);

        peutDasher = true;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Physic"))
    //    {
    //        speed *= 0.5f;
    //    }
    //    else if (!(collision.gameObject.layer == LayerMask.NameToLayer("Objects")))
    //    {
    //        if (vivant)
    //        {
    //            StartCoroutine(CMort());
    //        }
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Physic"))
    //    {
    //        speed *= 2.0f;
    //    }
    //}

    //IEnumerator CMort()
    //{
    //    if (!invincible)
    //    {
    //        vivant = false;
    //        mouvement = new Vector3(0.0f, 0.0f, 0.0f);
    //        rendu.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    //        Instantiate(animationMort, transform.position, Quaternion.identity);

    //        yield return new WaitForSeconds(0.8f);
    //        Instantiate(pouletRoti, transform.position, Quaternion.identity);

    //        yield return new WaitForSeconds(0.75f);
    //        sceneManager.GetComponent<GestionScene>().LoadSceneInitiale();
    //    }
    //}

    //public void ProvoquerMort()
    //{
    //    if (!invincible && vivant)
    //    {
    //        StartCoroutine(CMort());
    //    }
    //}

    //public void SetInvincible(bool active)
    //{
    //    invincible = active;
    //}

    //public bool GetVivant()
    //{
    //    return vivant;
    //}

}
