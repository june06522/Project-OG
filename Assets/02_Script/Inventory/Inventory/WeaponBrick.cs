using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponBrick : InvenBrick
{
    [SerializeField] private InvenWeapon weaponPrefab;

    private WeaponController weaponController;
    private InvenWeapon weapon;
    private Guid weaponGuid;

    protected override void Awake()
    {

        base.Awake();

        weaponController = GameManager.Instance.WeaponController;

    }

    public override void Settings()
    {
        weapon = Instantiate(weaponPrefab);
        InvenObject.OnSignalReceived += HandleWeaponSiganl;

        weaponGuid = weaponController.AddWeapon(weapon);
    }

    private void HandleWeaponSiganl(object obj)
    {

        weapon.GetSignal(obj);

    }


    public override void OnPointerDown(PointerEventData eventData)
    {

        base.OnPointerDown(eventData);

        if(weaponGuid != Guid.Empty)
        {

            Destroy(weapon.gameObject);
            weapon = null;
            weaponController.RemoveWeapon(weaponGuid);

        }

    }

}
