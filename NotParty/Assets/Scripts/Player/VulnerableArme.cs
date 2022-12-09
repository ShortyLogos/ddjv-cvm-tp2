using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableArme : MonoBehaviour
{
    [SerializeField]
    private string nom;

    public int degatAccumule; // ce � quoi l'on va comparer la progression
    public int vieMax; // ce � quoi l'on va comparer la progression
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
        if (degatAccumule >= vieMax)
        {
            Debug.Log("Niveau compl�t�.");
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
