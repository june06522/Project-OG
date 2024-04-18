using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyDashTransition : FSM_Transition<ETestEnemyState>
{
    TestEnemyDataSO data;
    public TestEnemyDashTransition(TestEnemyFSMController controller, ETestEnemyState nextState) : base(controller, nextState)
    {
        data = controller.EnemyDataSO as TestEnemyDataSO;
    }

    protected override bool CheckTransition()
    {
        return !data.IsDashCoolDown && data.IsJumpCoolDown;
    }
}
