using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoText;
    public TextMeshProUGUI totalAmmoText;
    public Image ammoTypeImage;

    [Header("Weapon")]
    public Image activeWeaponImage;
    public Image unActiveWeaponImage;

    [Header("Throwables")]
    public Image lethalImage;
    public TextMeshProUGUI lethalAmountText;
    public Image tacticalImage;
    public TextMeshProUGUI tacticalAmountText;

    public Sprite emptySlotSprite;


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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon != null)
        {


            magazineAmmoText.text = $"{activeWeapon.bulletsLeft}/{activeWeapon.magazineSize}";
            totalAmmoText.text = $"{activeWeapon.CheckAmmoLeft(activeWeapon.weaponModel)}";

            Weapon.WeaponModel model = activeWeapon.weaponModel;
            ammoTypeImage.sprite = GetAmmoSprite(model);

            activeWeaponImage.sprite = GetWeaponSprite(model);

            if (unActiveWeapon != null)
            {
                unActiveWeaponImage.sprite = GetWeaponSprite(unActiveWeapon.weaponModel);
            }
        }
        else
        {
            magazineAmmoText.text = "";
            totalAmmoText.text = "";

            ammoTypeImage.sprite = emptySlotSprite;
            activeWeaponImage.sprite = emptySlotSprite;
            unActiveWeaponImage.sprite = emptySlotSprite;
        }
    }

    public Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
                return Instantiate(Resources.Load<GameObject>("PistolAmmo")).GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Rifle:
                return Instantiate(Resources.Load<GameObject>("RifleAmmo")).GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Shotgun:
                return Instantiate(Resources.Load<GameObject>("ShotgunAmmo")).GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    public Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
                return Instantiate(Resources.Load<GameObject>("1911_Weapon")).GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Rifle:
                return Instantiate(Resources.Load<GameObject>("AK47_Weapon")).GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Shotgun:
                return Instantiate(Resources.Load<GameObject>("Shotgun")).GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    public GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }
}
