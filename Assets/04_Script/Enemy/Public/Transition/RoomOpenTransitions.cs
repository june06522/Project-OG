using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOpenTransitions<T> : BaseFSM_Transition<T> where T : Enum
{
    public RoomOpenTransitions(BaseFSM_Controller<T> controller, T nextState) : base(controller, nextState)
    {
    }

    protected override bool CheckTransition()
    {
        return true;
    }

}
