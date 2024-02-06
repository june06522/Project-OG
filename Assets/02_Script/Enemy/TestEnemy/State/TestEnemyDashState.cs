using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyDashState : TestEnemyRootState
{
    EnemyDataSO enemyDataSO;
    public TestEnemyDashState(TestEnemyController controller) : base(controller)
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
