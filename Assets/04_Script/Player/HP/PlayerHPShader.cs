using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPShader : MonoBehaviour
{
    PlayerHP _playerHP;
    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GameObject.Find("PlayerVisual").GetComponent<SpriteRenderer>();
        _playerHP = GameManager.Instance.player.GetComponent<PlayerHP>();
        _playerHP.OnChangeHPEvent += HandleUpdateHPUI;
        HandleUpdateHPUI(_playerHP.MaxHP, _playerHP.CurrentHP);
    }

    private void HandleUpdateHPUI(int maxHP, int currentHP)
    {
        _spriteRenderer.material.SetFloat("_FillAmount", (float)_playerHP.CurrentHP / (float)_playerHP.MaxHP);
    }
    private void OnDestroy()
    {
        _playerHP.OnChangeHPEvent -= HandleUpdateHPUI;
    }
}
