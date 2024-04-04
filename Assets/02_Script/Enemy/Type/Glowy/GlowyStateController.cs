using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowyStateController : BaseFSM_Controller<ENormalPatrolEnemyState>
{
    [NonSerialized]
    public Transform attackPoint;
    LaserBullet laserBullet;
    LaserPointer pointer;

    [SerializeField]
    private AudioClip _lazerClip;

    protected override void Awake()
    {
        base.Awake();
        attackPoint = transform.Find("AttackPoint").GetComponent<Transform>();
        laserBullet = transform.Find("LaserBullet").GetComponent<LaserBullet>();
        pointer = transform.Find("LaserPointer").GetComponent<LaserPointer>();
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

    public void SetLaserPointer(Vector2 endPos)
    {
        pointer.SetPos(attackPoint.position, endPos);
    }

    public void SetLaserPointerActive(bool value) => pointer.SetActive(value);

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
