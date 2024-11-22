using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
    public bool isActiveWeapon;

    //shooting
    public bool isFiring, readyToFire;
    bool allowreset = true;
    public float shootingDelay = 0.5f;

    //burst
    public int burstCount = 3;
    public int burstBulletsLeft;

    //spread
    public float spreadIntensity;

    //bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletLife = 2f;

    //public GameObject muzzleEffect;
    public AudioSource audioSource;
    public AudioClip shotSound;

    //loading
    public float reloadTime = 2f;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    internal Animator animator;

    public enum FireMode
    {
        Single,
        Burst,
        Auto
    }

    public enum WeaponModel
    {
        Pistol,
        Rifle,
        Shotgun,
    }

    public WeaponModel weaponModel;

    public FireMode fireMode;

    private void Awake()
    {
        readyToFire = true;
        burstBulletsLeft = burstCount;

        bulletsLeft = magazineSize;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveWeapon)
        {
            if (fireMode == FireMode.Auto)
            {
                isFiring = Input.GetKey(KeyCode.Mouse0);
            }
            else if (fireMode == FireMode.Single || fireMode == FireMode.Burst)
            {
                isFiring = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading && CheckAmmoLeft(weaponModel) > 0)
            {
                Reload();
            }

            if (bulletsLeft <= 0 && !isReloading && !isFiring && readyToFire)
            {
                Reload();
            }

            if (isFiring && readyToFire && bulletsLeft > 0 && !isReloading)
            {
                burstBulletsLeft = burstCount;
                Fire();
            }

            // if (AmmoManager.Instance.ammoText != null)
            // {
            //     if (fireMode == FireMode.Single)
            //     {
            //         AmmoManager.Instance.ammoText.text = $"{bulletsLeft}/{magazineSize}";
            //     }
            //     else if (fireMode == FireMode.Burst)
            //     {
            //         AmmoManager.Instance.ammoText.text = $"{bulletsLeft}/{magazineSize} ({burstCount})";
            //     }
            //     else if (fireMode == FireMode.Auto)
            //     {
            //         AmmoManager.Instance.ammoText.text = $"{bulletsLeft / burstCount}/{magazineSize / burstCount}";
            //     }
            //     // AmmoManager.Instance.ammoText.text = $"{bulletsLeft / burstCount}/{magazineSize / burstCount}";
            // }
        }
    }

    public int CheckAmmoLeft(WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case WeaponModel.Pistol:
                return WeaponManager.Instance.totalPistolAmmo;
            case WeaponModel.Rifle:
                return WeaponManager.Instance.totalRifleAmmo;
            case WeaponModel.Shotgun:
                return WeaponManager.Instance.totalShotgunAmmo;
            default:
                return 0;
        }
    }

    private void Fire()
    {

        animator.SetTrigger("RECOIL");

        bulletsLeft--;

        //muzzleEffect.GetComponent<ParticleSystem>().Play();
        if (audioSource != null && shotSound != null)
        {
            audioSource.PlayOneShot(shotSound);
        }

        readyToFire = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;
        // Rigidbody rb = bullet.GetComponent<Rigidbody>();
        // rb.velocity = bulletSpawn.forward * bulletVelocity;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBullet(bullet, bulletLife));

        if (allowreset)
        {
            Invoke("ResetFire", shootingDelay);
            allowreset = false;
        }

        if (fireMode == FireMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("Fire", shootingDelay);
        }
    }

    private void Reload()
    {
        isReloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        if (CheckAmmoLeft(weaponModel) >= magazineSize)
        {
            int bulletsUsed = magazineSize - bulletsLeft;
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsUsed, weaponModel);
        }
        else
        {
            bulletsLeft = CheckAmmoLeft(weaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, weaponModel);
        }
        isReloading = false;
    }

    private void ResetFire()
    {
        readyToFire = true;
        allowreset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float xSpread = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float ySpread = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(xSpread, ySpread, 0);
    }

    private IEnumerator DestroyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }


}
