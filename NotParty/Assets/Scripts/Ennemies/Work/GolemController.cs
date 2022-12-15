using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : WorkController
{
    [SerializeField]
    private float minTeleportationWait = 0f;
    [SerializeField]
    private float maxTeleportationWait = 2f;    
    [SerializeField]
    private GameObject fxTeleportation;
    [SerializeField]
    private AudioClip soundTeleportation;
    [SerializeField]
    private AudioSource audioSource;
    
    private GameObject[] obeliskList;
    private SpriteRenderer sprite;
    private BoxCollider2D col;
    private float timeDisappearAnim;
    private float timeReappearAnim;
    private float animTimeObelisk = 2.0f;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // Find Obelisks
        obeliskList = GameObject.FindGameObjectsWithTag("Obelisk");
        GameObject soundSource = GameObject.Find("GameHandling/UI/AudioSource");
        if (soundSource != null && audioSource == null) audioSource = soundSource.GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        timeDisappearAnim = specialAbilityDuration / 2;
        timeReappearAnim = specialAbilityDuration / 2;
    }

    public override float ActivateAbility()
    {
        GameObject destination = GetFurthestObelisk();
        float teleportationWait = Random.Range(minTeleportationWait, maxTeleportationWait);
        StartCoroutine(CTeleport(destination, teleportationWait));
        return specialAbilityDuration+teleportationWait+animTimeObelisk;
    }

    private IEnumerator CTeleport(GameObject destination, float delai)
    {
        StartCoroutine(CDisappear(destination));
        yield return new WaitForSeconds(delai+timeDisappearAnim);
        destination.GetComponent<Animator>().SetTrigger("Activate");
        yield return new WaitForSeconds(animTimeObelisk);
        transform.position = destination.transform.position;
        StartCoroutine(CReappear(destination));
    }

    private IEnumerator CDisappear(GameObject destination)
    {
        rig.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetTrigger("Disappear");
        yield return new WaitForSeconds(timeDisappearAnim);
        if (audioSource != null) audioSource.PlayOneShot(soundTeleportation);
        Instantiate(fxTeleportation, transform.position, Quaternion.identity);
        sprite.enabled = false;
        col.enabled = false;
    }

    private IEnumerator CReappear(GameObject destination)
    {
        if (audioSource != null) audioSource.PlayOneShot(soundTeleportation);
        Instantiate(fxTeleportation, destination.transform.position, Quaternion.identity);
        sprite.enabled = true;
        anim.SetTrigger("Reappear");
        yield return new WaitForSeconds(timeReappearAnim);
        col.enabled = true;
        rig.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private GameObject GetFurthestObelisk()
    {
        GameObject result = null;
        float maxDist = 0.0f;
        Vector3 player = GameObject.Find("Player").transform.position;
        foreach(GameObject obelisk in obeliskList)
        {
            float dist = Vector3.Distance(obelisk.transform.position, player);
            if (dist > maxDist)
            {
                result = obelisk;
                maxDist = dist;
            }
        }
        return result;
    } 
}
