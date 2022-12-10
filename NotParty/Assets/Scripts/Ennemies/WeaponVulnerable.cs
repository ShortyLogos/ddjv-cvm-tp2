using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVulnerable : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;
    private Color originalColor;

    [SerializeField]
    private string nom;

    [SerializeField]
    private bool isWork;
    private bool dead;

    public float damaged; // ce à quoi l'on va comparer la progression
    public float maxHealth; // ce à quoi l'on va comparer la progression

    // Start is called before the first frame updatawe
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        originalColor = sprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged >= maxHealth)
        {
            if (isWork && !GetComponent<WorkController>().IsDead())
            {
                Debug.Log("je passe ici");
                StartCoroutine(GetComponent<WorkController>().CVainquished());
            }
        }
    }

    public void Hit()
    {
        sprite.color = Color.white;
        StartCoroutine(COriginalColor());
    }

    IEnumerator COriginalColor()
    {
        yield return new WaitForSeconds(0.1f);
        sprite.color = originalColor;
    }
}
