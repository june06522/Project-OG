using FSM_System;
using UnityEngine;

public class MummyStateController : BaseFSM_Controller<ENormalEnemyState>
{

    public Transform target;
    
    [SerializeField]
    public Transform attackPoint;

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

        var attackState = new MummyAttackState(this);

        AddState(rootState, ENormalEnemyState.Idle);
        AddState(moveState, ENormalEnemyState.Move);
        AddState(attackState, ENormalEnemyState.Attack);
    }

}
