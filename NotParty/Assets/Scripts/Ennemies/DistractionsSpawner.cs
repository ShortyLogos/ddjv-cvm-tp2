using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionsSpawner : MonoBehaviour
{
    [SerializeField]
    private float minX, maxX, minY, maxY;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private GameObject[] list;
    [SerializeField]
    private GameObject sfxSpawn;

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
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector3 position = new Vector3(x, y, 0);
        GameObject distraction = Instantiate(prefab, position, Quaternion.identity, transform);
        Instantiate(sfxSpawn, distraction.transform.position, Quaternion.identity, distraction.transform);
    }

    private IEnumerator CSpawnerDistractions()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            SpawnDistraction();
        }
    }
}
