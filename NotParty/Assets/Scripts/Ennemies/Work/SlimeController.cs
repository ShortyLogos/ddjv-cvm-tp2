using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : WorkController
{
    [SerializeField]
    private AudioClip soundTornado;
    [SerializeField]
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        GameObject soundSource = GameObject.Find("GameHandling/UI/AudioSource");
        if (soundSource != null) audioSource = soundSource.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!dead)
        {
            anim.SetFloat("moveX", direction.x);
            if (!isMoving)
            {
                StartCoroutine(CMove());
            }
        }
    }

    public override float ActivateAbility()
    {
        anim.SetTrigger("SpecialAbility");
        if (audioSource != null) audioSource.PlayOneShot(soundTornado);
        return specialAbilityDuration;
    }
}
