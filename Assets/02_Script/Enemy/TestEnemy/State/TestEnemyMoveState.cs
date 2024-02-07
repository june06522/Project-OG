using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyMoveState : TestEnemyRootState
{
    public TestEnemyMoveState(TestEnemyFSMController controller) : base(controller)
    {

    }
    protected override void EnterState()
    {
        base.EnterState();
    }

    protected override void ExitState()
    {
        base.ExitState();
    }

    protected override void UpdateState()
    {
        base.UpdateState();
    }
}
