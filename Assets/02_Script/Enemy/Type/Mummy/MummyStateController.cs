using DG.Tweening;
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
    private SpriteRenderer spriteRender;
    [SerializeField]
    private Transform target;
    [SerializeField]
    public Transform attackPoint;


    protected override void Awake()
    {
        base.Awake();
        spriteRender = GetComponent<SpriteRenderer>();

        var patrolState = new MummyRootState(this);
        var patrolToMove = new TransitionIdleOrMove<EMummyState>(this, EMummyState.Move);

        patrolState
             .AddTransition<EMummyState>(patrolToMove);

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

    //Debug
    public void ChangeColor(Color color)
    {
        spriteRender.DOColor(color, 0.5f);
    }

    public void Flip(bool left)
    {
        transform.rotation = left ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
    }

    public void SetTarget(Vector2 targetPos)
    {
        target.position = targetPos;
    }

}
