using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : WorkController
{
    [SerializeField]
    private AudioClip soundAbility;
    [SerializeField]
    private AudioSource audioSource;
    private SpriteRenderer sprite;
    private Color originalColor;

    protected override void Start()
    {
        base.Start();
        if (audioSource == null)
        {
            GameObject soundSource = GameObject.Find("GameHandling/UI/AudioSource");
            if (soundSource != null) audioSource = soundSource.GetComponent<AudioSource>();
        }
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }

    public override float ActivateAbility() 
    {
        StartCoroutine(CIntangible());
        if (audioSource != null) audioSource.PlayOneShot(soundAbility);
        return specialAbilityDuration;
    }

    private IEnumerator CIntangible()
    {
        sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.25f);
        yield return new WaitForSeconds(specialAbilityDuration);
        sprite.color = originalColor;
        if (audioSource != null) audioSource.PlayOneShot(soundAbility);
    }

}
