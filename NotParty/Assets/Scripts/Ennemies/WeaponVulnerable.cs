using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVulnerable : MonoBehaviour
{
    [SerializeField]
    private string entityName;

    public float damaged; // ce à quoi l'on va comparer la progression
    public float maxHealth; // ce à quoi l'on va comparer la progression
    private SpriteRenderer sprite;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged >= maxHealth)
        {
            Debug.Log("Niveau complété.");
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
