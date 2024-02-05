using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyDashTransition : FSM_Transition<ETestEnemyState>
{
    public TestEnemyDashTransition(TestEnemyController controller, ETestEnemyState nextState) : base(controller, nextState)
    {

    }

    protected override bool CheckTransition()
    {
        return true;
    }
}
