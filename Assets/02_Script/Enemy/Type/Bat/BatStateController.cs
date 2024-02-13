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
    private SpriteRenderer spriteRender;

    protected override void Awake()
    {
        base.Awake();
        spriteRender = GetComponent<SpriteRenderer>();

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

    //Debug
    public void ChangeColor(Color color)
    {
        spriteRender.DOColor(color, 0.5f);
    }

    public void Flip(bool left)
    {
        transform.rotation = left ? Quaternion.identity : Quaternion.Euler(0,180,0);
    }
}
