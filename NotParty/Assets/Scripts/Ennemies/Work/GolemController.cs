using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : WorkController
{
    [SerializeField]
    private GameObject fxTeleportation;
    [SerializeField]
    private AudioClip soundTeleportation;
    [SerializeField]
    private AudioSource audioSource;
    
    private GameObject[] obeliskList;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // Find Obelisks
        obeliskList = GameObject.FindGameObjectsWithTag("Obelisk");
        GameObject soundSource = GameObject.Find("GameHandling/UI/AudioSource");
        if (soundSource != null) audioSource = soundSource.GetComponent<AudioSource>();
    }

    public override float ActivateAbility()
    {
        StartCoroutine(Teleport());
        return specialAbilityDuration;
    }

    private IEnumerator Teleport()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GameObject destination = GetFurthestObelisk();
        destination.GetComponent<Animator>().SetTrigger("Activate");
        anim.SetTrigger("SpecialAbility"); 
        yield return new WaitForSeconds(specialAbilityDuration/2);
        Instantiate(fxTeleportation, transform.position, Quaternion.identity);
        if (audioSource != null) audioSource.PlayOneShot(soundTeleportation);
        Instantiate(fxTeleportation, destination.transform.position, Quaternion.identity);
        transform.position = destination.transform.position;
        yield return new WaitForSeconds(specialAbilityDuration/2);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
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
