using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private Money _playerMoney;

    [Header("UI Info")]
    // value
    [SerializeField]
    private int _reRollGold = 2;
    [SerializeField]
    private int _reRollIncreaseGoldValue = 1;

    // ui object
    [SerializeField]
    private GameObject _shopUIObject;
    [SerializeField]
    private TextMeshProUGUI _playerGoldText;
    [SerializeField]
    private TextMeshProUGUI _reRollGoldText;

    // open
    private bool _isOpened = false;

    private void Awake()
    {
        _playerMoney = FindObjectOfType<Money>();
        if (_playerMoney == null)
            Debug.LogError("Money Object is not found");
    }

    public void OpenShop()
    {
        if(_isOpened) return;

        _isOpened = true;
        _playerMoney.GoldChangedEvent += UpdatePlayerGoldUI;
        _shopUIObject.SetActive(true);
    }

    public void CloseShop()
    {
        if (_isOpened == false) return;

        _isOpened = false;
        _playerMoney.GoldChangedEvent -= UpdatePlayerGoldUI;
        _shopUIObject.SetActive(false);
    }

    private void UpdatePlayerGoldUI(int gold)
    {
        _playerGoldText.text = $"{gold}G";
    }

    public void ReRoll()
    {
        if(_playerMoney.SpendGold(_reRollGold) == false) return;

        // 아이템 SO List 필요할 듯



        // 리롤마다 비용 증가
        _reRollGold += _reRollIncreaseGoldValue;
        _reRollGoldText.text = $"{_reRollGold}G";
    }
}
