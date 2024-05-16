using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincible : MonoBehaviour
{
    [SerializeField]
    PlayerHP _playerHP;

    private void Awake()
    {
        _playerHP.HitEvent += HandleInvincibleHP;
    }

    private void HandleInvincibleHP(int damage)
    {
        int currentHP = _playerHP.CurrentHP;
        int maxHP = _playerHP.MaxHP;

        if(currentHP <= 0)
        {
            _playerHP.SetPlayerHP(maxHP, 1);

        }
    }
}
