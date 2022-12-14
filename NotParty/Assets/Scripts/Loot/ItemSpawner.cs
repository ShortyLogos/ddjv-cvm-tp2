using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rdm = UnityEngine.Random;
using System;

public class ItemSpawner : MonoBehaviour
{
    enum ListType
    {
        Items,
        Weapons
    }


    [SerializeField]
    private float minX, maxX, minY, maxY;
    [SerializeField]
    private float spawnZoneX, spawnZoneY, spawnZoneSize;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float radiusCheck;
    [SerializeField]
    int maxAttempts = 100;
    [SerializeField]
    private GameObject glowingEffect;

    [Space(30)]
    [SerializeField][Tooltip("List all items that can be spawn and their rarity.")]
    private Item[] itemsList;
    [SerializeField]
    private float ItemSpawnCooldown;
    [SerializeField]
    private float lifeSpan;

    [Space(30)]
    [SerializeField][Tooltip("List all weapons that can be spawn and their rarity.")]
    private Item[] weaponsList;
    [SerializeField]
    private float weaponsSpawnCooldown;

    private float totalRatioWeapons, totalRatioItems;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Item weapon in weaponsList)
        {
            totalRatioWeapons += weapon.Rarity;
        }

        foreach (Item item in itemsList)
        {
            totalRatioItems += item.Rarity;
        }

        //Make sure there's a weapon around the starting position for the player to pick early
        GameObject startingWeapon = GetRandomItem(ListType.Weapons).Prefab;
        PlaceObject(startingWeapon, new Vector3(Rdm.Range(spawnZoneX, spawnZoneX+spawnZoneSize), Rdm.Range(spawnZoneY,spawnZoneY+spawnZoneSize), 0));
        
        StartCoroutine(CSpawnerWeapons());
        StartCoroutine(CSpawnerItems());
    }

    private void SpawnWeapon()
    {
        GameObject weapon = GetRandomItem(ListType.Weapons).Prefab;
        PlaceObject(weapon);
    }

    //Spawn the items on the player directly
    public void GiveLoot(GameObject loot)
    {
        Vector3 position = GameObject.Find("Map/Player").transform.position;
        PlaceObject(loot, position, false);
    }

    private void SpawnItem()
    {
        GameObject item = GetRandomItem(ListType.Items).Prefab;
        GameObject instance = PlaceObject(item);
        StartCoroutine(CDestroyItem(instance));
    }

    private Item GetRandomItem(ListType type)
    {
        switch(type)
        {
            case ListType.Weapons:
                {
                    float x = Rdm.Range(0, totalRatioWeapons);
                    foreach(Item weapon in weaponsList)
                    {
                        if ((x -= weapon.Rarity) < 0)
                        {
                            return weapon;
                        }
                    }
                    break;
                }
            case ListType.Items:
                {
                    float x = Rdm.Range(0, totalRatioItems);
                    foreach(Item item in itemsList)
                    {
                        if ((x -= item.Rarity) < 0)
                        {
                            return item;
                        }
                    }
                    break;
                }
            default:
                {
                    throw new Exception("Must specify a list type.");
                }
        }
        return new Item();
    }

    private GameObject PlaceObject(GameObject itemPrefab)
    {
        Vector3 position = getValidPosition(Vector3.zero);
        GameObject loot = Instantiate(itemPrefab, position, Quaternion.identity, transform);
        Instantiate(glowingEffect, loot.transform.position, Quaternion.identity, loot.transform);
        return loot;
    }

    private GameObject PlaceObject(GameObject itemPrefab, Vector3 wantedPosition, bool validation = true)
    {
        if (validation)
        {
            wantedPosition = getValidPosition(wantedPosition);
        }
        GameObject loot = Instantiate(itemPrefab, wantedPosition, Quaternion.identity, transform);
        Instantiate(glowingEffect, loot.transform.position, Quaternion.identity, loot.transform);
        return loot;
    }

    private Vector3 getValidPosition(Vector3 wantedPosition)
    {
        int attempts = 0;
        bool validPlacement;
        if (wantedPosition == Vector3.zero)
        {
            validPlacement = false;
        }
        else
        {
            validPlacement = Physics2D.OverlapCircle(wantedPosition, radiusCheck, layerMask) == null;
        }
        while (!validPlacement)
        {
            attempts++;
            float x = Rdm.Range(minX, maxX);
            float y = Rdm.Range(minY, maxY);
            wantedPosition = new Vector3(x, y, 0);
            validPlacement = Physics2D.OverlapCircle(wantedPosition, radiusCheck, layerMask) == null;
            if (attempts > maxAttempts)
            {
                Debug.LogWarning($"{attempts} attempts were made to spawn an item, should probably reduce the radius check variable.");
                wantedPosition = Vector3.zero;
                break;
            }
        }
        return wantedPosition;
    }

    private IEnumerator CSpawnerWeapons ()
    {
        while(true)
        {
            yield return new WaitForSeconds(weaponsSpawnCooldown);
            SpawnWeapon();
        }
    }

    private IEnumerator CSpawnerItems()
    {
        while(true)
        {
            yield return new WaitForSeconds(ItemSpawnCooldown);
            SpawnItem();
        }

    }

    private IEnumerator CDestroyItem(GameObject item)
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(item);
    }

}

//==============================================================
// Item Class
//==============================================================

[Serializable]
public struct Item
{
    public Item(float rarity, GameObject prefab)
    {
        this.rarity = rarity;
        this.prefab = prefab;
    }

    [SerializeField] [Range(0, 1)]
    private float rarity;

    [SerializeField]
    private GameObject prefab;

    public GameObject Prefab 
    {
        get => prefab;
    }

    public float Rarity
    {
        get => rarity;
    }
}