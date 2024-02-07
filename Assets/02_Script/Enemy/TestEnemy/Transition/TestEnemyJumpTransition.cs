using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyJumpTransition : FSM_Transition<ETestEnemyState>
{
    TestEnemyDataSO data;
    public TestEnemyJumpTransition(TestEnemyFSMController controller, ETestEnemyState nextState) : base(controller, nextState)
    {
        data = controller.EnemyData;
    }

    protected override bool CheckTransition()
    {
        return !data.IsJumpCoolDown;
    }
}
