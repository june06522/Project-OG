using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSkeletonStateController : BaseFSM_Controller<EMummyState>
{
    public Transform target;

    [SerializeField]
    private EnemyBullet _bullet;
    [SerializeField]
    private Transform _weapon;

    protected override void Start()
    {
        base.Start();

        var rootState = new MummyRootState(this);
        rootState
            .AddTransition<EMummyState>(new RoomOpenTransitions<EMummyState>(this, EMummyState.Patrol));

        var patrolState = new MummyPatrolState(this);
        var patrolToMove = new TransitionIdleOrMove<EMummyState>(this, EMummyState.Move);

        patrolState
           .AddTransition<EMummyState>(patrolToMove);

        var moveState = new MummyMoveState(this);
        var moveToIdle = new TransitionIdleOrMove<EMummyState>(this, EMummyState.Patrol);
        var moveToAttack = new MoveToAttackTransition<EMummyState>(this, EMummyState.Attack, true);

        moveState
            .AddTransition<EMummyState>(moveToIdle)
            .AddTransition<EMummyState>(moveToAttack);

        // ½ºÄÌ·¹Åæ ¾îÅÃ
        var attackState = new BulletAttackState(this, _bullet, _weapon);

        AddState(rootState, EMummyState.Idle);
        AddState(patrolState, EMummyState.Patrol);
        AddState(moveState, EMummyState.Move);
        AddState(attackState, EMummyState.Attack);
    }
}
