using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public List<GameObject> weaponList = new List<GameObject>();
    public GameObject initialWeapon;

    // Start is called before the first frame update
    void Start()
    {
        if (initialWeapon != null)
        {
            addWeapon(initialWeapon);
        }
    }

    public void addWeapon(GameObject weapon)
    {
        PlayerController player = GetComponent<PlayerController>();
        player.PlaySound(player.pickUpSound);
        bool newWeapon = true;
        string weaponName = weapon.GetComponent<WeaponType>().weaponName;

        int i = 0;
        int index = 0;
        foreach (GameObject alreadyPossessed in weaponList)
        {
            if (alreadyPossessed.GetComponent<WeaponType>().weaponName.Equals(weaponName) && newWeapon)
            {
                index = i;
                newWeapon = false;
            }

            i++;
        }

        if (newWeapon)
        {
            weaponList.Add(weapon);
            this.gameObject.GetComponent<CurrentWeapon>().NewWeapon();
        }
        else
        {
            this.gameObject.GetComponent<CurrentWeapon>().SwitchWeapon(index);
        }
    }
}
