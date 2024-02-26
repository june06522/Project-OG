using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFSM_Transition<T> : FSM_Transition<T> where T : Enum
{
    protected new BaseFSM_Controller<T> controller;
    protected EnemyDataSO _data => controller.EnemyData;
    public BaseFSM_Transition(BaseFSM_Controller<T> controller, T nextState) : base(controller, nextState)
    {
        this.controller = controller;   
    }

    protected override bool CheckTransition()
    {
        return true;
    }
}
