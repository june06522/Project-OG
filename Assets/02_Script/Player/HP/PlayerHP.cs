using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour, IHitAble
{
    public FeedbackPlayer feedbackPlayer { get; set; }

    // maxHP, currentHP
    public event Action<int, int> OnChangeHPEvent;

    // damage
    public event Action<int> HitEvent;
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }

    private bool _isDead = false;

    public bool Hit(float damage)
    {
        if (_isDead) return false;

        feedbackPlayer?.Play(damage);
        CurrentHP -= (int)damage;
        OnChangeHPEvent?.Invoke(MaxHP, CurrentHP);
        HitEvent?.Invoke((int)damage);

        if (CurrentHP <= 0)
        {
            Die();
            return false;
        }
         
        return true;
    }

    public void RestoreHP(int plusHP)
    {
        CurrentHP = Math.Clamp(CurrentHP + plusHP, 0, MaxHP);
        OnChangeHPEvent?.Invoke(MaxHP, CurrentHP);
    }
    public void SetPlayerHP(int maxHP, int currentHP = 0)
    {
        MaxHP = maxHP;

        if (currentHP == 0)
        {

            CurrentHP = maxHP;

        }
        else
        {
            CurrentHP = currentHP;
        }

        OnChangeHPEvent?.Invoke(MaxHP, CurrentHP);
    }

    private void Die()
    {
        _isDead = true;
    }
}
