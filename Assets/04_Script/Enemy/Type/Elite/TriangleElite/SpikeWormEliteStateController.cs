using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;

public class SpikeWormEliteStateController : BaseFSM_Controller<ENormalEnemyState>
{
    protected override void Start()
    {
        base.Start();

        var rootState = new NormalRootState(this);
        rootState
            .AddTransition<ENormalEnemyState>(new RoomOpenTransitions<ENormalEnemyState>(this, ENormalEnemyState.Move));

        var moveState = new NormalChaseState(this);
        var moveToAttack = new MoveToAttackTransition<ENormalEnemyState>(this, ENormalEnemyState.Attack, true);

        moveState
            .AddTransition<ENormalEnemyState>(moveToAttack);

        var attackState = new SpikeWormEliteAttackState(this);

        AddState(rootState, ENormalEnemyState.Idle);
        AddState(moveState, ENormalEnemyState.Move);
        AddState(attackState, ENormalEnemyState.Attack);
    }
}
