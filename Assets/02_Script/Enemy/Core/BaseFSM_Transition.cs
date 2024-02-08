using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFSM_Transition<T> : FSM_Transition<T> where T : Enum
{
    public BaseFSM_Transition(BaseFSM_Controller<T> controller, T nextState) : base(controller, nextState)
    {
        this.controller = controller;
        gameObject = controller.gameObject;
        transform = controller.transform;
        this.nextState = nextState;
    }

    protected override bool CheckTransition()
    {
        return true;
    }
}
