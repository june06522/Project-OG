using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameTooltip : MonoBehaviour
{

    RectTransform _rectTransform;
    Image _backgroundImage;

    TextMeshProUGUI _nameText;
    TextMeshProUGUI _rateText;
    TextMeshProUGUI _explainText;

    Item _lastItem = null;
    bool _isOn = false;

    private void Awake()
    {

        _rectTransform = GetComponent<RectTransform>();
        _backgroundImage = GetComponent<Image>();

        _nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        _rateText = transform.Find("Rate").GetComponent<TextMeshProUGUI>();
        _explainText = transform.Find("Explain").GetComponent<TextMeshProUGUI>();

    }

    public void SetInfo(Item item)
    {

        if (_isOn && _lastItem != null && _lastItem == item)
        {

            SetPos(item.UIPos);
            return;

        }

        InvenBrick brick = item.Brick;

        // check
        if (brick == null || brick.Type == ItemType.Connector)
            return;

        // Save Info
        _lastItem = item;

        // On UI
        SetOnOff(true);

        // Set Pos
        SetPos(item.UIPos);

        // Set Info
        switch (brick.Type)
        {
            case ItemType.Weapon:
                SetWeaponInfo(brick);
                SetColor(brick.ItemRate);
                break;
            case ItemType.Generator:
                SetGeneratorInfo(brick);
                SetColor(brick.ItemRate);
                break;
            case ItemType.Connector:
                break;
        }

        

    }

    private void SetOnOff(bool isOn)
    {
        _isOn = isOn;

        _backgroundImage.enabled    = isOn;
        _nameText.enabled           = isOn;
        _rateText.enabled           = isOn;
        _explainText.enabled        = isOn;
    }

    private void SetPos(Vector3 itemPos)
    {
        _rectTransform.position = itemPos;

        //Vector3 screenPos = Camera.main.WorldToScreenPoint(itemPos);
        //_rectTransform.position = screenPos;
    }

    private void SetWeaponInfo(InvenBrick brick)
    {
        WeaponBrick weaponBrick = brick as WeaponBrick;

        if(weaponBrick != null)
        {

            WeaponID id = weaponBrick.WeaponPrefab.id;
            ItemRate rate = weaponBrick.ItemRate;

            string nameText = WeaponExplainManager.weaponName[id];
            string rateText = WeaponExplainManager.itemRate[rate];
            string explainText = WeaponExplainManager.weaponExplain[id];

            _nameText.text = nameText;
            _rateText.text = rateText;
            _explainText.text = explainText;

        }
        
    }

    private void SetGeneratorInfo(InvenBrick brick)
    {
        GeneratorID id = brick.InvenObject.generatorID;
        ItemRate rate = brick.ItemRate;

        string nameText = WeaponExplainManager.generatorName[id];
        string rateText = WeaponExplainManager.itemRate[rate];
        string explainText = WeaponExplainManager.generatorExplain[id];

        _nameText.text = nameText;
        _rateText.text = rateText;
        _explainText.text = explainText;
    }

    public void OffInfo()
    {

        _lastItem = null;
        _isOn = false;

        // Off UI
        SetOnOff(false);

    }

    private void SetColor(ItemRate rate)
    {
        switch (rate)
        {
            case ItemRate.NORMAL:
                _rateText.color = Color.white;
                break;
            case ItemRate.RARE:
                _rateText.color = Color.cyan;
                break;
            case ItemRate.EPIC:
                _rateText.color = Color.magenta;
                break;
            case ItemRate.LEGEND:
                _rateText.color = Color.yellow;
                break;
        }
    }

}
