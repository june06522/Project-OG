using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    PlayerHP _playerHP;

    [Header("PlayerHP UI Info")]
    [SerializeField]
    TextMeshProUGUI _hpText;
    [SerializeField]
    Slider _hpSlider;

    private void Start()
    {
        _playerHP = GameManager.Instance.player.GetComponent<PlayerHP>();
        _playerHP.OnChangeHPEvent += HandleUpdateHPUI;
        HandleUpdateHPUI(_playerHP.MaxHP, _playerHP.CurrentHP);
    }

    private void HandleUpdateHPUI(int maxHP, int currentHP)
    {
        _hpText.text = $"{currentHP}/{maxHP}";
        _hpSlider.value = currentHP / (float)maxHP;
    }
}
