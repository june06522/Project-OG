using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyJumpTransition : FSM_Transition<ETestEnemyState>
{
    public TestEnemyJumpTransition(TestEnemyController controller, ETestEnemyState nextState) : base(controller, nextState)
    {

    }

    protected override bool CheckTransition()
    {
        return true;
    }
}
