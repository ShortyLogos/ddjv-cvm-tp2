using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoueurControleur : MonoBehaviour
{
    private Rigidbody2D rig;
    private SpriteRenderer rendu;
    //private Animator anim;
    private bool vivant = true;
    private bool startedMoving = false;
    private bool invincible;

    [SerializeField]
    private Vector3 mouvement = new Vector3(0.0f, 0.0f, 0.0f);

    //[SerializeField]
    //private GameObject 
            //animationMort, 
            //pouletRoti, 
            //sceneManager;

    [SerializeField]
    private float speed;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        rendu = GetComponent<SpriteRenderer>();
        //sceneManager = GameObject.FindWithTag("SceneManager");

        invincible = false;
        mouvement.z = 0.0f;
    }

    void Update()
    {
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(inputX) > 0.01f || Mathf.Abs(inputY) > 0.01f)
            {
                startedMoving = true;
            }

            if (vivant && startedMoving)
            {
                mouvement.x = inputX;
                mouvement.y = inputY;
            }

            //anim.SetFloat("moveX", mouvement.x);
            //anim.SetFloat("moveY", mouvement.y);

            float speed = 0.0f;
            if (startedMoving)
            {
                speed = mouvement.sqrMagnitude;
            }
            //anim.SetFloat("Speed", speed);

            //if (mouvement.x == 1 || mouvement.x == -1 || mouvement.y == 1 || mouvement.y == -1)
            //{
            //    anim.SetFloat("lastMoveX", mouvement.x);
            //    anim.SetFloat("lastMoveY", mouvement.y);
            //}
        }
    }

    void FixedUpdate()
    {
        if (startedMoving)
        {
            rig.velocity = mouvement.normalized * speed;
        }
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

    //public void Celebrate()
    //{
    //    StartCoroutine(CCelebration());
    //}

    //IEnumerator CCelebration()
    //{
    //    invincible = true;
    //    vivant = false;
    //    speed = 0.25f;
    //    mouvement = new Vector3(-1.0f, 0.0f, 0.0f);
    //    yield return new WaitForSeconds(3.0f);
    //    mouvement = new Vector3(0.0f, 0.0f, 0.0f);
    //    yield return new WaitForSeconds(5.0f);
    //    mouvement = new Vector3(1.0f, 0.0f, 0.0f);
    //    yield return new WaitForSeconds(6.0f);
    //    mouvement = new Vector3(0.0f, 0.0f, 0.0f);
    //    yield return new WaitForSeconds(5.0f);
    //    mouvement = new Vector3(-1.0f, 0.0f, 0.0f);
    //    yield return new WaitForSeconds(3.0f);
    //    mouvement = new Vector3(0.0f, 0.0f, 0.0f);
    //    GameObject.FindWithTag("SceneManager").GetComponent<GestionScene>().LoadProchaineScene(true);
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
