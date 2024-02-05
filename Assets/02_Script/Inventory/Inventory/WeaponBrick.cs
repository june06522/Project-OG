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

    public override void Setting()
    {
        Debug.Log(GameManager.Instance.InventoryActive);
        bool isControllerSetActive = GameManager.Instance.InventoryActive.inven.activeSelf == false;
        
        if(isControllerSetActive)
        {
            GameManager.Instance.InventoryActive.inven.SetActive(true);
            Debug.Log("1");
        }

        weapon = Instantiate(weaponPrefab);
        InvenObject.OnSignalReceived += HandleWeaponSiganl;

        Debug.Log($"{weaponController}----");

        weaponGuid = weaponController.AddWeapon(weapon);

        if(isControllerSetActive)
            GameManager.Instance.InventoryActive.inven.SetActive(false);
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
