using DG.Tweening;
using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatStateController : BaseFSM_Controller<ENormalEnemyState>
{
    [SerializeField] public Transform attackPoint;

    protected override void Start()
    {
        base.Start();
        var idleState = new NormalRootState(this);
        var idleToMove = new TransitionIdleOrMove<ENormalEnemyState>(this, ENormalEnemyState.Move);

        idleState
            .AddTransition<ENormalEnemyState>(idleToMove);

        var moveState = new NormalChaseState(this);
        var moveToAttack = new MoveToAttackTransition<ENormalEnemyState>(this, ENormalEnemyState.Attack, false);

        moveState
            .AddTransition<ENormalEnemyState>(moveToAttack);

        var attackState = new BatAttackState(this);

        AddState(idleState, ENormalEnemyState.Idle);
        AddState(moveState, ENormalEnemyState.Move);
        AddState(attackState, ENormalEnemyState.Attack);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHP player;
        if(collision.transform.TryGetComponent<PlayerHP>(out player))
        {
            player.Hit(EnemyDataSO.AttackPower);
        }
    }
}
