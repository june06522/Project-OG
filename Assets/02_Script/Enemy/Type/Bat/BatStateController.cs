using DG.Tweening;
using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBatState
{
    Idle = 0,
    Move = 1,
    Attack
}

public class BatStateController : BaseFSM_Controller<EBatState>
{
    [SerializeField] public Transform attackPoint;

    protected override void Awake()
    {
        base.Awake();
        var idleState = new BatEnemyRootState(this);
        var idleToMove = new TransitionIdleOrMove<EBatState>(this, EBatState.Move);

        idleState
            .AddTransition<EBatState>(idleToMove);

        var moveState = new BatMoveState(this);
        var moveToIdle = new TransitionIdleOrMove<EBatState>(this, EBatState.Idle);
        var moveToAttack = new MoveToAttackTransition<EBatState>(this, EBatState.Attack, false);

        moveState
            .AddTransition<EBatState>(moveToIdle)
            .AddTransition<EBatState>(moveToAttack);

        var attackState = new BatAttackState(this);

        AddState(idleState, EBatState.Idle);
        AddState(moveState, EBatState.Move);
        AddState(attackState, EBatState.Attack);
    }
}
