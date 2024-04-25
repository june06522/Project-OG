using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;

public class FourthEliteStateController : BaseFSM_Controller<ENormalPatrolEnemyState>
{
    public GameObject bullet;

    protected override void Start()
    {
        base.Start();

        var rootState = new NormalPatrolRootState(this);
        var rootToPatrol = new RoomOpenTransitions<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Patrol);
        rootState
            .AddTransition<ENormalPatrolEnemyState>(rootToPatrol);

        var patrolState = new NormalPatrolState(this);
        var patrolToMove = new PatrolToChaseTransition<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Move);
        patrolState
             .AddTransition<ENormalPatrolEnemyState>(patrolToMove);

        var moveState = new NormalPatrolChaseStae(this);
        var moveToAttack = new MoveToAttackTransition<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Attack, true);
        moveState
            .AddTransition<ENormalPatrolEnemyState>(moveToAttack);

        var attackState = new FourthEliteAttackState(this);

        AddState(rootState, ENormalPatrolEnemyState.Idle);
        AddState(patrolState, ENormalPatrolEnemyState.Patrol);
        AddState(moveState, ENormalPatrolEnemyState.Move);
        AddState(attackState, ENormalPatrolEnemyState.Attack);
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
