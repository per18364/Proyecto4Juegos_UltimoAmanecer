using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoveredWeapon;
    public AmmoBox hoveredAmmobox;
    public TextMeshProUGUI ammoText;

    public void Awake()
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
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.GetComponent<Weapon>() && hitObject.GetComponent<Weapon>().isActiveWeapon == false)
            {
                // Debug.Log("Weapon found");
                hoveredWeapon = hitObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickUpWeapon(hitObject.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                    // hoveredWeapon = null;
                }
            }

            //AmmoBox
            if (hitObject.GetComponent<AmmoBox>())
            {
                hoveredAmmobox = hitObject.GetComponent<AmmoBox>();
                hoveredAmmobox.GetComponent<Outline>().enabled = true;
                ammoText.gameObject.SetActive(true);
                if (hoveredAmmobox.ammoType == AmmoBox.AmmoType.PistolAmmo)
                {
                    ammoText.text = $"Presiona E para recoger {hoveredAmmobox.ammoAmount} balas de pistola.";
                }
                else if (hoveredAmmobox.ammoType == AmmoBox.AmmoType.RifleAmmo)
                {
                    ammoText.text = $"Presiona E para recoger {hoveredAmmobox.ammoAmount} balas de rifle.";
                }
                else if (hoveredAmmobox.ammoType == AmmoBox.AmmoType.ShotgunAmmo)
                {
                    ammoText.text = $"Presiona E para recoger {hoveredAmmobox.ammoAmount} balas de escopeta.";
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickUpAmmo(hoveredAmmobox);
                    Destroy(hitObject.gameObject);
                }
            }
            else
            {
                if (hoveredAmmobox)
                {
                    ammoText.gameObject.SetActive(false);
                    hoveredAmmobox.GetComponent<Outline>().enabled = false;
                    // hoveredWeapon = null;
                }
            }
        }
    }
}
