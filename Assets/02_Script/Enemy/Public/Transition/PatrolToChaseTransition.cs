using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolToChaseTransition<T> : BaseFSM_Transition<T> where T : Enum
{
    Transform targetTrm;
    public PatrolToChaseTransition(BaseFSM_Controller<T> controller, T nextState) : base(controller, nextState)
    {
        targetTrm = controller.Target;
    }

    protected override bool CheckTransition()
    {
        // 거리 안에 있고 공격 할수 있는 상태일때.
        return Transitions.CheckDistance(targetTrm, this.transform, _data.Range) 
            && !_data.IsAttackCoolDown;
    }
}
