using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DistractionsSpawner : MonoBehaviour
{
    [SerializeField]
    private float minX, maxX, minY, maxY;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float radiusCheck;
    [SerializeField]
    int maxAttempts = 100;
    [SerializeField]
    private float minCooldown;
    [SerializeField]
    private float maxCooldown;
    [SerializeField]
    private GameObject[] list;
    [SerializeField]
    private GameObject sfxSpawn;
    private UnityAction<object> lowLifeWork;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CSpawnerDistractions());
    }

    private void SpawnDistraction()
    {
        GameObject distraction = list[Random.Range(0,list.Length)];
        PlaceObject(distraction);
    }

    private void PlaceObject(GameObject prefab)
    {
        int attempts = 0;
        bool validPlacement = false;
        Vector3 position = Vector3.zero;
        while (!validPlacement)
        {
            attempts++;
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            position = new Vector3(x, y, 0);
            validPlacement = Physics2D.OverlapCircle(position, radiusCheck, layerMask) == null;
            if (attempts > maxAttempts)
            {
                Debug.LogWarning($"{attempts} attempts were made to spawn a distraction, should probably reduce placement's radius.");
                break;
            }
        }
        GameObject distraction = Instantiate(prefab, position, Quaternion.identity, transform);
        Instantiate(sfxSpawn, distraction.transform.position, Quaternion.identity, distraction.transform);
    }

    private IEnumerator CSpawnerDistractions()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minCooldown, maxCooldown));
            SpawnDistraction();
        }
    }

    private void Awake()
    {
        lowLifeWork = new UnityAction<object>(Accelerate);
    }

    private void OnEnable()
    {
        EventManager.StartListening("LorsPause", lowLifeWork);
    }

    private void OnDisable()
    {
        EventManager.StopListening("LorsPause", lowLifeWork);
    }

    private void Accelerate(object data)
    {
        maxCooldown /= 2;
    }
}
