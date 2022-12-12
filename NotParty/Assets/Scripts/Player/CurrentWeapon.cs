using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ref : https://www.youtube.com/watch?v=7c68z05vaX4

public class CurrentWeapon : MonoBehaviour
{
    public GameObject weapon;

    private int weaponIndex = 0;
    private WeaponInventory inventory;

    private GameHandler gameHandler;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<WeaponInventory>();
        GameObject handler = GameObject.FindGameObjectWithTag("GameHandler");
        if (handler != null) gameHandler = handler.GetComponent<GameHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && weapon != null)
        {
            weapon.GetComponent<ParticleSystem>().Stop();
        }

        if (Input.GetMouseButtonDown(0) && weapon != null)
        {
            weapon.GetComponent<ParticleSystem>().Play();
        }

        bool cycleLeft = Input.GetKeyDown(KeyCode.Q);
        bool cycleRight = Input.GetKeyDown(KeyCode.E);
        if (cycleRight || cycleLeft)
        {
            if (weapon != null)
            {
                weapon.GetComponent<ParticleSystem>().Stop();
            }

            if (inventory.weaponList.Count > 0)
            {
                if (cycleRight)
                {
                    weaponIndex++;
                }
                else
                {
                    weaponIndex--;
                }

                weaponIndex = weaponIndex % inventory.weaponList.Count;

                weapon = inventory.weaponList[Mathf.Abs(weaponIndex)];
                UpdateIcon();
            }

            if (Input.GetMouseButton(0) && weapon != null)
            {
                weapon.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    public void SetWeapon(GameObject newWeapon)
    {
        bool isShooting = false;
        if (weapon != null && weapon.GetComponent<ParticleSystem>().isPlaying)
        {
            isShooting = true;
            weapon.GetComponent<ParticleSystem>().Stop();
        }
        weapon = newWeapon;
        UpdateIcon();

        if (isShooting || Input.GetMouseButton(0))
        {
            weapon.GetComponent<ParticleSystem>().Play();
        }
    }

    private void UpdateIcon()
    {
        if (gameHandler != null) gameHandler.ChangeWeapon(weapon.GetComponent<WeaponType>().weaponName);
    }

    public void NewWeapon()
    {
        SetWeapon(inventory.weaponList.Last());
        weaponIndex = inventory.weaponList.Count - 1;
    }

    public void SwitchWeapon(int index)
    {
        weaponIndex = index;
        SetWeapon(inventory.weaponList[index]);
    }
}
