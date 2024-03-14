using FSM_System;
using UnityEngine;

public enum EMummyState
{
    Idle = 0,
    Patrol = 1,
    Move = 2,
    Attack
}

public class MummyStateController : BaseFSM_Controller<EMummyState>
{

    public Transform target;
    
    [SerializeField]
    public Transform attackPoint;

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

        var attackState = new MummyAttackState(this);

        AddState(rootState, EMummyState.Idle);
        AddState(patrolState, EMummyState.Patrol);
        AddState(moveState, EMummyState.Move);
        AddState(attackState, EMummyState.Attack);
    }
}
