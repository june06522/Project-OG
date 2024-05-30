using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabStateController : BaseFSM_Controller<ECrabEnemyState>
{
    protected override void Start()
    {
        //base.Start();

        
        //var rootState = new CrabRootState(this);
        //var rootToPatrol = new RoomOpenTransitions<ECrabEnemyState>(this, ECrabEnemyState.Patrol);
        //rootState
        //    .AddTransition<ECrabEnemyState>(rootToPatrol);

        //var patrolState = new CrabPatrolState(this);
        //var patrolToMove = new MoveToAttackTransition<ECrabEnemyState>(this, ECrabEnemyState.Attack, false);
        //patrolState
        //     .AddTransition<ECrabEnemyState>(patrolToMove);

        //var attackState = new CrabAttackState(this);

        //AddState(rootState, ECrabEnemyState.Idle);
        //AddState(patrolState, ECrabEnemyState.Patrol);
        //AddState(attackState, ECrabEnemyState.Attack);
    }
}
