using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 인벤토리에 들어오는 아이템 이미지
// 여기서 플레이어한테 달린 아이템 찾아서 스킬 작동시켜야함

public class WeaponBrick : InvenBrick
{
    [SerializeField] private InvenWeapon weaponPrefab;

    private WeaponController weaponController;
    private InvenWeapon weapon;
    private Guid weaponGuid;

    protected override void Awake()
    {

        base.Awake();

        weaponController = FindObjectOfType<WeaponController>();

    }

    public override void Setting()
    {

        weapon = Instantiate(weaponPrefab);
        Debug.Log("이건가");
        InvenObject.OnSignalReceived -= HandleWeaponSiganl;
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
