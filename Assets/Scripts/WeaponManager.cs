using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalPistolAmmo = 0;
    public int totalRifleAmmo = 0;
    public int totalShotgunAmmo = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if(weaponSlots.Count > 0)
        {
            activeWeaponSlot = weaponSlots[0];
            if (activeWeaponSlot != null)
            {
                Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
                newWeapon.isActiveWeapon = true;
            }
        }
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }
        activeWeaponSlot = weaponSlots[slotNumber];
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    public void PickUpWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponToActiveSlot(pickedUpWeapon);
    }

    public void AddWeaponToActiveSlot(GameObject weapon)
    {
        DropCurrentWeapon(weapon);

        weapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon newWeapon = weapon.GetComponent<Weapon>();
        weapon.transform.localPosition = new Vector3(newWeapon.spawnPosition.x, newWeapon.spawnPosition.y, newWeapon.spawnPosition.z);
        weapon.transform.localRotation = Quaternion.Euler(newWeapon.spawnRotation.x, newWeapon.spawnRotation.y, newWeapon.spawnRotation.z);

        newWeapon.isActiveWeapon = true;
        newWeapon.animator.enabled = true;
    }

    public void DropCurrentWeapon(GameObject weapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var currentWeapon = activeWeaponSlot.transform.GetChild(0).gameObject;

            currentWeapon.GetComponent<Weapon>().isActiveWeapon = false;
            currentWeapon.GetComponent<Weapon>().animator.enabled = false;

            currentWeapon.transform.SetParent(weapon.transform.parent);
            currentWeapon.transform.localPosition = weapon.transform.localPosition;
            currentWeapon.transform.localRotation = weapon.transform.localRotation;
        }
    }

    public void PickUpAmmo(AmmoBox ammoBox)
    {
        switch (ammoBox.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammoBox.ammoAmount;
                break;
            case AmmoBox.AmmoType.RifleAmmo:
                totalRifleAmmo += ammoBox.ammoAmount;
                break;
        }
        Destroy(ammoBox.gameObject);
    }

    public void DecreaseTotalAmmo(int bulletsUsed, Weapon.WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case Weapon.WeaponModel.Pistol:
                totalPistolAmmo -= bulletsUsed;
                break;
            case Weapon.WeaponModel.Rifle:
                totalRifleAmmo -= bulletsUsed;
                break;
            case Weapon.WeaponModel.Shotgun:
                totalShotgunAmmo -= bulletsUsed;
                break;
        }
    }
}
