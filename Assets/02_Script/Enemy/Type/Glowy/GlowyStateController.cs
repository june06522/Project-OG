using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGlowyState
{
    Idle = 0,
    Patrol = 1,
    Move = 2,
    Attack,
}

public class GlowyStateController : BaseFSM_Controller<EGlowyState>
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

        var rootState = new GlowyRootState(this);
        var rootToPatrol = new RoomOpenTransitions<EGlowyState>(this, EGlowyState.Patrol);
        rootState
            .AddTransition<EGlowyState>(rootToPatrol);

        var patrolState = new GlowyPatrolState(this);
        var patrolToMove = new PatrolToChaseTransition<EGlowyState>(this, EGlowyState.Move);
        patrolState
             .AddTransition<EGlowyState>(patrolToMove);

        var moveState = new GlowyChaseState(this);
        var moveToAttack = new MoveToAttackTransition<EGlowyState>(this, EGlowyState.Attack, true);
        moveState
            .AddTransition<EGlowyState>(moveToAttack);

        var attackState = new GlowyAttackState(this);

        AddState(rootState, EGlowyState.Idle);
        AddState(patrolState, EGlowyState.Patrol);
        AddState(moveState, EGlowyState.Move);
        AddState(attackState, EGlowyState.Attack);
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
