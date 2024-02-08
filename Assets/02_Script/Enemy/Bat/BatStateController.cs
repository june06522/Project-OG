using DG.Tweening;
using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBatState
{
    Idle,
    Chase,
    Attack
}

public class BatStateController : FSM_Controller<EBatState>
{
    [field: SerializeField] public EnemyDataSO EnemyData { get; protected set; }
    [SerializeField] public Transform attackPoint;
    public SpriteRenderer spriteRender;

    Enemy enemy;

    protected override void Awake()
    {
        //spriteRender = GetComponent<SpriteRenderer>();

        //enemy = GetComponent<Enemy>();
        //EnemyData = Instantiate(EnemyData);

        //var idleState = new BatEnemyRootState(this);
        //var idleToChase = new TransitionIdleOrChase(this, EBatState.Chase);

        //idleState
        //    .AddTransition<EBatState>(idleToChase);

        //var chaseState = new BatChaseState(this);
        //var chaseToIdle = new TransitionIdleOrChase(this, EBatState.Idle);
        //var chaseToAttack = new BatChaseToAttackTransition(this, EBatState.Attack);

        //chaseState
        //    .AddTransition<EBatState>(chaseToIdle)
        //    .AddTransition<EBatState>(chaseToAttack);

        //var attackState = new BatAttackState(this);

        //AddState(idleState, EBatState.Idle);
        //AddState(chaseState, EBatState.Chase);
        //AddState(attackState, EBatState.Attack);
    }

    protected override void Update()
    {
        if (enemy.Dead) return;
        base.Update();
    }

    //Debug
    public void ChangeColor(Color color)
    {
        spriteRender.DOColor(color, 0.5f);
    }
}
