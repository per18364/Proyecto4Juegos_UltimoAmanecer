using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount;
    public AmmoType ammoType;

    public enum AmmoType
    {
        PistolAmmo,
        RifleAmmo,
        ShotgunAmmo
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
