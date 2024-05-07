using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;

public class FifthEliteStateController : BaseFSM_Controller<ENormalEnemyState>
{
    [SerializeField]
    private Enemy enemy;

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

        var attackState = new FifthEliteAttackState(this);

        AddState(idleState, ENormalEnemyState.Idle);
        AddState(moveState, ENormalEnemyState.Move);
        AddState(attackState, ENormalEnemyState.Attack);
    }

    protected override void Update()
    {
        base.Update();

        ShieldDown();
    }

    private void ShieldDown()
    {
        if(enemy.CurHP < enemy.EnemyDataSO.MaxHP / 2)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHP player;
        if (collision.transform.TryGetComponent<PlayerHP>(out player))
        {
            player.Hit(EnemyDataSO.AttackPower);
        }
    }
}
