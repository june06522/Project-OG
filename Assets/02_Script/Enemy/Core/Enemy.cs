using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour, IHitAble
{
    [SerializeField] EnemyDataSO enemyDataSO;
    public EnemyDataSO EnemyDataSO => enemyDataSO;
    public FeedbackPlayer feedbackPlayer { get; set; }
    public bool Dead { get; private set; } = false;
    private int curHp;

    public event Action DeadEvent;
    private new Collider2D collider;

    public Collider2D Collider => collider;

    public Transform TargetTrm { get; set; }
    public RoomInfo RoomInfo { get; private set; } //내가 지금 위치해있는 room정보;

    //Debug
    [SerializeField]
    private Tilemap _mainMap;
    [SerializeField]
    private Tilemap _wallMap;

    private void Awake()
    {
        curHp = enemyDataSO.MaxHP;
        DeadEvent += DieEvent;
        collider = GetComponent<Collider2D>();
        TargetTrm = GameObject.Find("Player").GetComponent<Transform>();

        //Debug 나중에 한곳에서 할당해줘야함.
        Debug.Log(_mainMap.cellBounds);

        RoomInfo roomInfo = new RoomInfo()
        {
            bound = _mainMap.cellBounds,
            pos = _mainMap.transform.position,
        };
        
        SetRoomInfo(roomInfo);
    }

    public void SetRoomInfo(RoomInfo curRoom)
    {
        RoomInfo = curRoom;
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
