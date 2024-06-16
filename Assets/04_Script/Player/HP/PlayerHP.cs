using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour, IHitAble
{
    [field: SerializeField]
    public FeedbackPlayer feedbackPlayer { get; set; }

    // maxHP, currentHP
    public event Action<int, int> OnChangeHPEvent;

    // damage
    public event Action<int> HitEvent;
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }

    private float defence = 0;
    private bool _isDead = false;

    private void Start()
    {

        SynergyManager.Instance.OnSynergyChange += ChangeDefenceFactor;

    }

    private void ChangeDefenceFactor()
    {

        defence = SynergyManager.Instance.GetStatFactor(TriggerID.Idle);

    }

    public bool Hit(float damage)
    {
        if (_isDead) return false;

        damage = (1 - defence) * damage;

        EventTriggerManager.Instance?.HitExecute();
        feedbackPlayer?.Play(damage);
        CurrentHP -= (int)damage;
        HitEvent?.Invoke((int)damage);
        OnChangeHPEvent?.Invoke(MaxHP, CurrentHP);

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

        SceneManager.LoadScene("DieScene");
    }

    private void OnDestroy()
    {
        SynergyManager.Instance.OnSynergyChange -= ChangeDefenceFactor;
    }
}
