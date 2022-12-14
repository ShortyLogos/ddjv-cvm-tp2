using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVulnerable : MonoBehaviour
{
    private SpriteRenderer sprite;

    [SerializeField]
    private string nom;

    [SerializeField]
    private bool isWork;

    [SerializeField]
    private float damageTaken;
    [SerializeField]
    private float maxHealth;

    private GameHandler gameHandler;
    private WorkController controller;

    [SerializeField]
    private float nbrThreshold = 4.0f;
    private int indexThreshold = 1;
    private float threshold = 0.0f;
    private bool immune = false;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        GameObject handler = GameObject.FindGameObjectWithTag("GameHandler");
        if (handler != null && isWork)
        {
            gameHandler = handler.GetComponent<GameHandler>();
            gameHandler.SetMaxProgress(maxHealth);
            threshold = maxHealth / nbrThreshold;
        }
        sprite = GetComponent<SpriteRenderer>();
        if (isWork)
        {
            controller = GetComponent<WorkController>();
            float difficulty = PlayerPrefs.GetFloat("Difficulty", 1);
            float scale = difficulty / 10;
            maxHealth += maxHealth * scale;
        }
        originalColor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (damageTaken >= maxHealth)
        {
            if (isWork)
            {
                StartCoroutine(controller.CVainquished());
            } else
            {
                GetComponent<Ennemy>().Defeat();
            }
        } else if (isWork && damageTaken >= 0.75*maxHealth)
        {
            EventManager.TriggerEvent("LowLife", (damageTaken, maxHealth));
        }
    }

    public void Heal(float amount)
    {
        damageTaken = Mathf.Clamp(damageTaken -= amount, 0.0f, maxHealth);
        if (gameHandler != null)
        {
            gameHandler.SetBackProgress(amount);
        }
    }

    public void Hit(float amount)
    {
        if (!immune)
        {
            damageTaken = Mathf.Clamp(damageTaken += amount, 0.0f, maxHealth);
            if (isWork)
            {
                int test = ((int)(damageTaken / (threshold * indexThreshold)));
                if (indexThreshold < nbrThreshold && test>=1)
                {
                    indexThreshold++;
                    immune = true;
                    float immunityDuration = controller.ActivateAbility();
                    StartCoroutine(CInvincible(immunityDuration));
                }
                if (gameHandler != null)
                {
                    gameHandler.Progress(amount);
                }
            }
            StartCoroutine(CFlashColor());
        }
    }

    private IEnumerator CInvincible(float duration)
    {
        yield return new WaitForSeconds(duration);
        immune = false;
    }

    IEnumerator CFlashColor()
    {
        if (nom != "Pepe Montinus" || !immune)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            if (nom != "Pepe Montinus" || !immune)
            {
                sprite.color = originalColor;
            }
        }
    }
}
