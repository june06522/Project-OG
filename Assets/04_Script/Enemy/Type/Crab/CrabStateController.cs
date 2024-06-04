using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabStateController : BaseFSM_Controller<ENormalPatrolEnemyState>
{
    public List<Transform> LaserAttackPoints;
    
    protected override void Start()
    {
        base.Start();

        var rootState = new NormalPatrolRootState(this);
        var rootToPatrol = new RoomOpenTransitions<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Patrol);
        rootState
            .AddTransition<ENormalPatrolEnemyState>(rootToPatrol);
       
        var patrolState = new CrabPatrolState(this);
        var patrolToMove = new PatrolToChaseTransition<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Move);
        patrolState
             .AddTransition<ENormalPatrolEnemyState>(patrolToMove);

        var moveState = new NormalPatrolChaseStae(this);
        var moveToAttack = new MoveToAttackTransition<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Attack, true);
        moveState
            .AddTransition<ENormalPatrolEnemyState>(moveToAttack);

        var attackState = new CrabAttackState(this);

        AddState(rootState, ENormalPatrolEnemyState.Idle);
        AddState(patrolState, ENormalPatrolEnemyState.Patrol);
        AddState(moveState, ENormalPatrolEnemyState.Move);
        AddState(attackState, ENormalPatrolEnemyState.Attack);
    }
}
