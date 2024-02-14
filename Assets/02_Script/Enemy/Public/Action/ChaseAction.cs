using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction<T> : BaseAction<T> where T : Enum
{
    Transform targetTrm;
    public ChaseAction(BaseFSM_Controller<T> controller, Transform targetTrm) : base(controller)
    {
        this.targetTrm = GameManager.Instance.player;
    }

    public override void OnEnter()
    {
        //Debug
        controller.ChangeColor(Color.blue);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        Vector3 dir = (targetTrm.position - controller.transform.position);
        dir.z = 0;
        float speed = _data.Speed;

        if (dir.magnitude > _data.AttackAbleRange)
        {
            controller.transform.position += dir.normalized * speed * Time.deltaTime;
        }

        controller.Flip(dir.x < 0);
    }
}
