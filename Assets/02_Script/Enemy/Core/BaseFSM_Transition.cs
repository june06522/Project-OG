using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFSM_Transition<T> : FSM_Transition<T> where T : Enum
{
    protected EnemyDataSO _data;
    public BaseFSM_Transition(BaseFSM_Controller<T> controller, T nextState) : base(controller, nextState)
    {
        _data = controller.EnemyData;
    }

    protected override bool CheckTransition()
    {
        return true;
    }
}
