using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyDashTransition : FSM_Transition<ETestEnemyState>
{
    TestEnemyDataSO data;
    public TestEnemyDashTransition(TestEnemyController controller, ETestEnemyState nextState) : base(controller, nextState)
    {
        data = controller.EnemyData;
    }

    protected override bool CheckTransition()
    {
        return !data.IsDashCoolDown && data.IsJumpCoolDown;
    }
}
