using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class BaseAlert : MonoBehaviour
{
    private UnityAction<object> whenAlert;
    
    [SerializeField]
    private float alertTime = 3.5f;
    
    [SerializeField]
    private GameObject[] explosionsPositions;
    [SerializeField]
    private GameObject explosionPrefab;
    
    private BoxCollider2D[] doors;
    private Animator[] decorations;
    
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        decorations = GetComponentsInChildren<Animator>();
        doors = GetComponentsInChildren<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        whenAlert = new UnityAction<object>(Alert);
    }

    private void OnEnable()
    {
        EventManager.StartListening("Alert", whenAlert);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Alert", whenAlert);
    }

    private void Alert(object data)
    {
        foreach (Animator decoration in decorations)
        {
            decoration.SetTrigger("Alert");
        }
        foreach (BoxCollider2D door in doors)
        {
            StartCoroutine(CActivateDoor(door));
        }
        StartCoroutine(CSoundAlert());
        StartCoroutine(CExplosions());
    }

    private IEnumerator CSoundAlert()
    {
        audioSource.Play();
        yield return new WaitForSeconds(alertTime);
        audioSource.Stop();
    }

    private IEnumerator CActivateDoor(BoxCollider2D door)
    {
        
        yield return new WaitForSeconds(1);
        door.enabled = true;
        yield return new WaitForSeconds(alertTime-1);
        door.enabled = false;
    }

    private IEnumerator CExplosions()
    {
        foreach (GameObject pos in explosionsPositions)
        {
            Instantiate(explosionPrefab, pos.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(alertTime/explosionsPositions.Length);
        }
    }

}
