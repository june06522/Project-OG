using FSM_System;
using UnityEngine;

public enum EMummyState
{
    Idle = 0,
    Move = 1,
    Attack
}

public class MummyStateController : BaseFSM_Controller<EMummyState>
{

    public Transform target;
    
    [SerializeField]
    public Transform attackPoint;

    protected override void Awake()
    {
        base.Awake();

        var patrolState = new MummyRootState(this);
        var patrolToMove = new TransitionIdleOrMove<EMummyState>(this, EMummyState.Move);

        //patrolState
        //   .AddTransition<EMummyState>(patrolToMove);

        var moveState = new MummyMoveState(this);
        var moveToIdle = new TransitionIdleOrMove<EMummyState>(this, EMummyState.Idle);
        var moveToAttack = new MoveToAttackTransition<EMummyState>(this, EMummyState.Attack, true);

        moveState
            .AddTransition<EMummyState>(moveToIdle)
            .AddTransition<EMummyState>(moveToAttack);

        var attackState = new MummyAttackState(this);

        AddState(patrolState, EMummyState.Idle);
        AddState(moveState, EMummyState.Move);
        AddState(attackState, EMummyState.Attack);
    }
}
