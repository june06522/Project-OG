using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitAble
{
    [SerializeField] EnemyDataSO enemyDataSO;
    public FeedbackPlayer feedbackPlayer { get; set; }
    public bool Dead { get; private set; } = false;
    private int curHp;

    public event Action DeadEvent;

    private void Awake()
    {
        curHp = enemyDataSO.MaxHP;
        DeadEvent += DieEvent;
    }

    public void Hit(float damage)
    {
        if (Dead) return;

        curHp -= (int)damage;

        if (curHp <= 0)
        {
            Dead = true;
            Die();
            return;
        }

        feedbackPlayer.Play(damage + UnityEngine.Random.Range(0.25f, 1.75f));
    }

    private void Die()
    {
        Debug.Log("Die");

        DeadEvent?.Invoke();
    }

    private void DieEvent()
    {
        if(MonsterSpawnManager.Instance != null)
        {
            MonsterSpawnManager.Instance.monsterSpawn.curMonsterCnt--;
        }
        else
        {
            Debug.LogError($"MonsterSpawnManager is not created!");
        }

        Destroy(gameObject);
    }
}
