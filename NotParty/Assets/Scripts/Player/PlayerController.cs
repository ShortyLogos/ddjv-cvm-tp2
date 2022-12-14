using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rig;
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

    // Durée de cooldown pour le UI
    [SerializeField]
    private float dashCooldown = 1.35f;

    [SerializeField]
    private float speed = 3.5f;

    [SerializeField]
    private AudioSource audioSource;
    public AudioClip pickUpSound;

    private float SpeedMultiplier { get; set; }
    private float EfficiencyMultiplier { get; set; }

    private GameHandler gameHandler;
    private bool gamePaused = false;
    private UnityAction<object> lorsPause;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dashTrail = GetComponent<TrailRenderer>();
        GameObject handler = GameObject.FindWithTag("GameHandler");
        if (handler != null) gameHandler = handler.GetComponent<GameHandler>();
        GameObject soundSource = GameObject.Find("GameHandling/UI/AudioSource");
        if (soundSource != null && audioSource == null) audioSource = soundSource.GetComponent<AudioSource>();
        dashing = false;
        SpeedMultiplier = 1.0f;
        EfficiencyMultiplier = 1.0f;
        mouvement.z = 0.0f;
    }

    void Update()
    {
        if (!gamePaused)
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
    }

    void FixedUpdate()
    {
        if (startedMoving & !gamePaused)
        {
            if (!dashing)
            {
                rig.velocity = mouvement.normalized * speed * SpeedMultiplier;
            }
            else
            {
                rig.AddForce(mouvement.normalized * speed * dashSpeedMultiplier * SpeedMultiplier, ForceMode2D.Force);
            }
        }
    }

    // Methods about speed multiplier
    public void AddSpeed(float value, bool playSound = true)
    {
        if (playSound) PlaySound(pickUpSound);
        SpeedMultiplier += value;
    }

    public void RemoveSpeed(float value)
    {
        SpeedMultiplier -= value;
    }

    public float GetSpeedMultiplier()
    {
        return SpeedMultiplier;
    }

    // Methods about efficiency multiplier
    public void AddEfficiency(float value, bool playSound = true)
    {
        if (playSound) PlaySound(pickUpSound);
        EfficiencyMultiplier += value;
    }

    public void RemoveEfficiency(float value)
    {
        EfficiencyMultiplier = Mathf.Max(0.01f, EfficiencyMultiplier -= value);
    }

    public float GetEfficiencyMultiplier()
    {
        return EfficiencyMultiplier;
    }

    IEnumerator CDash()
    {
        dashing = true;
        canDash = false;
        dashTrail.emitting = true;
        anim.speed = 1.75f;
        if (gameHandler != null) gameHandler.ActivateCooldownUI(dashCooldown, "Dash");
        yield return new WaitForSeconds(dashDuration);

        dashing = false;
        dashTrail.emitting = false;
        anim.speed = 1.0f;
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    public void PlaySound(AudioClip sound)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }


    private void Awake()
    {
        lorsPause = new UnityAction<object>(LorsPause);
    }

    private void OnEnable()
    {
        EventManager.StartListening("LorsPause", lorsPause);
    }

    private void OnDisable()
    {
        EventManager.StopListening("LorsPause", lorsPause);
    }

    private void LorsPause(object data)
    {
        gamePaused ^= true;
    }
}
