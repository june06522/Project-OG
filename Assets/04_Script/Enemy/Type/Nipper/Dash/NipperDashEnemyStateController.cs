using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;

public class NipperDashEnemyStateController : BaseFSM_Controller<ENormalEnemyState>
{
    public Animator animator;

    public readonly int dehisce = Animator.StringToHash("Dehisce");
    public readonly int shut = Animator.StringToHash("Shut");
    public readonly int idle = Animator.StringToHash("Idle");

    public GameObject eye;

    [HideInInspector] public bool isStop = false; 

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

        var attackState = new NipperDashEnemyAttackState(this);

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

        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isStop = true;
        }
    }
}
