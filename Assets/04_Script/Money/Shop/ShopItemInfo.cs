using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemInfo : MonoBehaviour
{
    [Header("UI Info")]
    [SerializeField]
    private Image _itemInfoPanel;

    [Header("Text UI Info")]
    [SerializeField]
    private TextMeshProUGUI _itemName;
    [SerializeField]
    private TextMeshProUGUI _itemValue;
    [SerializeField]
    private TextMeshProUGUI _itemExplain;

    public void SetPos(Vector3 worldPos)
    {

        _itemInfoPanel.rectTransform.position = worldPos;

    }

    public void SetInfo(ItemInfoSO itemInfoSO)
    {
        SetEnableUI(true);

        InvenBrick brick = itemInfoSO.Brick;

        switch (brick.Type)
        {
            case ItemType.Weapon:
                SetWeaponInfo(brick);
                break;
            case ItemType.Generator:
                SetGeneratorInfo(brick);
                break;
            case ItemType.Connector:
                break;
        }

    }

    public void SetEnableUI(bool value)
    {

        if (_itemInfoPanel.gameObject.activeInHierarchy == value)
            return;

        _itemInfoPanel.gameObject.SetActive(value);

    }

    private void SetWeaponInfo(InvenBrick brick)
    {
        WeaponBrick weaponBrick = brick as WeaponBrick;

        if (weaponBrick != null)
        {

            WeaponID id = weaponBrick.WeaponPrefab.id;

            string nameText = WeaponExplainManager.weaponName[id];

            _itemName.text = nameText;

            WeaponDataSO weaponData = weaponBrick.WeaponPrefab.Data;
            string valueText = $"공격력 {weaponData.AttackDamage.GetValue()}, 공격속도 {weaponData.AttackCoolDown.GetValue()}";

            _itemValue.text = valueText;

            _itemExplain.text = WeaponExplainManager.weaponExplain[id];

        }
    }

    private void SetGeneratorInfo(InvenBrick brick)
    {
        GeneratorID id = brick.InvenObject.generatorID;

        string nameText = WeaponExplainManager.generatorName[id];

        _itemName.text = nameText;

        TriggerID triggerID = WeaponExplainManager.triggerExplain[id];
        string triggerName = WeaponExplainManager.triggerName[triggerID];

        _itemValue.text = triggerName;
        _itemExplain.text = WeaponExplainManager.generatorExplain[id];
    }

}
