using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 인벤토리에 들어오는 아이템 이미지

public class WeaponBrick : InvenBrick
{
    [SerializeField] private InvenWeapon _weaponPrefab;
    public InvenWeapon WeaponPrefab => _weaponPrefab;

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
        weapon = Instantiate(_weaponPrefab);
        //Debug.Log("이건가");
        InvenObject.OnSignalReceived -= HandleWeaponSiganl;
        InvenObject.OnSignalReceived += HandleWeaponSiganl;

        weaponGuid = weaponController.AddWeapon(weapon);
    }

    private void HandleWeaponSiganl(object obj)
    {

        if(obj != null)
            weapon.GetSignal(obj);

    }


    public override void OnPointerDown(PointerEventData eventData)
    {

        base.OnPointerDown(eventData);

        if (weaponGuid != Guid.Empty)
        {
            if(weapon != null)
            Destroy(weapon.gameObject); 
            weapon = null;
            weaponController.RemoveWeapon(weaponGuid);

        }

    }

    public override void ShowExplain(Vector2 invenPoint)
    {
        if (isHover == true) return;
        isHover = true;

        ItemExplain.Instance.HoverWeapon(invenPoint, image.sprite, GetName(), GetDamage(), GetExplain(), GetOnSkillList(),GetEvaluation());
    }

    private float GetDamage() => _weaponPrefab.Data.GetDamage();

    private string GetName() => _weaponPrefab.GetName();

    private string GetExplain() => _weaponPrefab.explainTxt;

    //private string GetEvaluation() => WeaponExplainManager.itemRate[itemRate];
    private ItemRate GetEvaluation() => itemRate;

    private Tuple<GeneratorID, int>[] GetOnSkillList()
    {
        List<Tuple<GeneratorID, int>> list = InventoryWeaponInfo.Instance.GetConnect(InvenObject.originPos.x, InvenObject.originPos.y);

        return list.ToArray();
    }
}