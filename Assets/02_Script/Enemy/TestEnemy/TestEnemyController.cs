using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETestEnemyState
{
    Idle,
    Move,
    Jump,
    Dash,
}

public class TestEnemyController : FSM_Controller<ETestEnemyState>
{
    [field: SerializeField] public TestEnemyDataSO EnemyData { get; protected set; }

    protected override void Awake()
    {
        EnemyData = Instantiate(EnemyData);

        var idleState = new TestEnemyRootState(this);
        var idleToMove = new TestEnemyTransitionToMoveOrIdle(this, ETestEnemyState.Move);
        
        idleState
            .AddTransition<ETestEnemyState>(idleToMove);
        
        var moveState = new TestEnemyMoveState(this);
        var moveToIdle = new TestEnemyTransitionToMoveOrIdle(this, ETestEnemyState.Idle);
        var goToDash = new TestEnemyDashTransition(this, ETestEnemyState.Dash);
        var goToJump = new TestEnemyJumpTransition(this, ETestEnemyState.Jump);

        moveState
            .AddTransition<ETestEnemyState>(moveToIdle)
            .AddTransition<ETestEnemyState>(goToJump)
            .AddTransition<ETestEnemyState>(goToDash);

        var dashState = new TestEnemyDashState(this);
        var jumpState = new TestEnemyJumpState(this);
        
        AddState(idleState, ETestEnemyState.Idle);
        AddState(moveState, ETestEnemyState.Move);
        AddState(dashState, ETestEnemyState.Dash);
        AddState(jumpState, ETestEnemyState.Jump);
    }
}
