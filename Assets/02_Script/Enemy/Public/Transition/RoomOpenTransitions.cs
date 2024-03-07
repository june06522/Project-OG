using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOpenTransitions<T> : BaseFSM_Transition<T> where T : Enum
{
    DetectZone myZone;
    public RoomOpenTransitions(BaseFSM_Controller<T> controller, T nextState, DetectZone detectZone) : base(controller, nextState)
    {
        myZone = detectZone;
    }

    protected override bool CheckTransition()
    {
        return myZone.IsPlayerIn;
    }

}
