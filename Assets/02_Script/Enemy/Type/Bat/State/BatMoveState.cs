using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMoveState : BatEnemyRootState
{
    Transform targetTrm;
    public BatMoveState(BatStateController controller) : base(controller)
    {
        targetTrm = GameManager.Instance.player;
    }

    protected override void EnterState()
    {
        //Debug
        controller.ChangeColor(Color.blue);
    }

    protected override void ExitState()
    {
        
    }

    protected override void UpdateState()
    {
        Vector3 dir = (targetTrm.position - controller.transform.position);
        dir.z = 0;
        float speed = _data.Speed;

        if(dir.magnitude < _data.AttackAbleRange)
        {
            
        }
        else
        {
            controller.transform.position += dir.normalized * speed * Time.deltaTime;
        }

        controller.Flip(dir.x < 0);
    }
}
