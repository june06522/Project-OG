using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [SerializeField] private Weapon debugWeapon;

    private List<Weapon> weaponContainer = new();

    private void Update()
    {
        
        if(debugWeapon != null && Input.GetKeyDown(KeyCode.L))
        {

            AddWeapon(Instantiate(debugWeapon));

        }

    }

    public Guid AddWeapon(Weapon weapon)
    {

        weaponContainer.Add(weapon);
        return weapon.WeaponGuid;

    }

    public void RemoveWeapon(Guid guid)
    {

        var removeWeapon = weaponContainer.Find(x => x.WeaponGuid == guid);

        if (removeWeapon != null)
        {

            weaponContainer.Remove(removeWeapon);

        }

    }

}
