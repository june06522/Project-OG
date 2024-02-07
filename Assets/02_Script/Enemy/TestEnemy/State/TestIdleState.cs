using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIdleState : FSM_State<ETestEnemyState>
{

    public TestIdleState(FSM_Controller<ETestEnemyState> controller) : base(controller)
    {
    }
}
