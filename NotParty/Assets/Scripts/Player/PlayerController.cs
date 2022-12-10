using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rig;
    private SpriteRenderer sprite;
    private Animator anim;
    private TrailRenderer dashTrail;

    private bool startedMoving = false;
    private bool dashing;
    private bool canDash = true;

    [SerializeField]
    private Vector3 mouvement = new Vector3(0.0f, 0.0f, 0.0f);

    [SerializeField]
    private float dashSpeedMultiplier = 4.0f;

    [SerializeField]
    private float dashDuration = 0.5f;

    [SerializeField]
    private float dashCooldown = 1.35f;

    [SerializeField]
    private float speed = 3.5f;

    private float speedMultiplier { get; set; }
    private float efficiencyMultiplier { get; set; }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        dashTrail = GetComponent<TrailRenderer>();
        //sceneManager = GameObject.FindWithTag("SceneManager");

        dashing = false;
        speedMultiplier = 1.0f;
        efficiencyMultiplier = 1.0f;
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

        if (startedMoving)
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

        if (Input.GetKeyDown(KeyCode.Space) && !dashing && canDash)
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
                rig.velocity = mouvement.normalized * speed * speedMultiplier;
            }
            else
            {
                rig.AddForce(mouvement.normalized * speed * dashSpeedMultiplier * speedMultiplier, ForceMode2D.Force);
            }
        }
    }

    // Methods about speed multiplier
    public void AddSpeed(float value)
    {
        speedMultiplier += value;
    }

    public void RemoveSpeed(float value)
    {
        speedMultiplier -= value;
    }

    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }

    // Methods about efficiency multiplier
    public void AddEfficiency(float value)
    {
        efficiencyMultiplier += value;
    }

    public void RemoveEfficiency(float value)
    {
        efficiencyMultiplier -= value;
    }

    public float GetEfficiencyMultiplier()
    {
        return efficiencyMultiplier;
    }

    IEnumerator CDash()
    {
        dashing = true;
        canDash = false;
        dashTrail.emitting = true;
        anim.speed = 1.75f;
        yield return new WaitForSeconds(dashDuration);

        dashing = false;
        dashTrail.emitting = false;
        anim.speed = 1.0f;
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

}
