using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GlowyStateController : BaseFSM_Controller<ENormalPatrolEnemyState>
{
    [NonSerialized]
    public Transform attackPoint;
    
    LaserBullet laserBullet;

    public LaserPointer[] pointers = new LaserPointer[2];

    [SerializeField]
    private AudioClip _lazerClip;

    protected override void Awake()
    {
        base.Awake();
        attackPoint = transform.Find("AttackPoint").GetComponent<Transform>();
        laserBullet = transform.Find("LaserBullet").GetComponent<LaserBullet>();
        pointers = transform.Find("LaserPointers").GetComponentsInChildren<LaserPointer>();
    }

    protected override void Start()
    {
        base.Start();

        var rootState = new NormalPatrolRootState(this);
        var rootToPatrol = new RoomOpenTransitions<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Patrol);
        rootState
            .AddTransition<ENormalPatrolEnemyState>(rootToPatrol);

        var patrolState = new NormalPatrolState(this);
        var patrolToMove = new PatrolToChaseTransition<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Move);
        patrolState
             .AddTransition<ENormalPatrolEnemyState>(patrolToMove);

        var moveState = new NormalPatrolChaseStae(this);
        var moveToAttack = new MoveToAttackTransition<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Attack, true);
        moveState
            .AddTransition<ENormalPatrolEnemyState>(moveToAttack);

        var attackState = new GlowyAttackState(this);

        AddState(rootState, ENormalPatrolEnemyState.Idle);
        AddState(patrolState, ENormalPatrolEnemyState.Patrol);
        AddState(moveState, ENormalPatrolEnemyState.Move);
        AddState(attackState, ENormalPatrolEnemyState.Attack);
    }

    public void SetLaserPointer(Vector2 endPos, int index)
    { 
        pointers[index].SetPos(attackPoint.position, endPos);
    }

    public void SetLaserPointerActive(bool value)
    {
        for(int i = 0; i < pointers.Length; i++) 
        {
            pointers[i].SetActive(value);
        }
    }

    public void ResetPoints()
    {
        for (int i = 0; i < pointers.Length; i++)
        {
            pointers[i].ResetPoint();
        }
    }

    public void Shoot(Vector2 endPos)
    {
        SoundManager.Instance.SFXPlay("Lazer", _lazerClip);
        laserBullet.Shoot(attackPoint.position, endPos, EnemyDataSO.AttackPower, false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
