using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 인벤토리에 들어오는 아이템 이미지

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

    // 장착 시 실행
    public override void Settings()
    {
        weapon = Instantiate(weaponPrefab);
        //Debug.Log("이건가");
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

        if (weaponGuid != Guid.Empty)
        {

            Destroy(weapon.gameObject);
            weapon = null;
            weaponController.RemoveWeapon(weaponGuid);

        }

    }

    public override void ShowExplain()
    {
        ItemExplain.Instance.HoverWeapon(image.sprite, GetName(), GetDamage(), GetExplain(), GetOnSkillList());
    }

    private float GetDamage() => weaponPrefab.Data.AttackDamage.GetValue();

    private string GetName() => weaponPrefab.GetName();

    private string GetExplain() => weaponPrefab.explainTxt;

    private string[] GetOnSkillList()
    {
        //List<string> list = InventoryWeaponInfo.Instance.GetConnect(InvenObject.originPos.x, InvenObject.originPos.y);

        //return list.ToArray();
        return null;
    }
}