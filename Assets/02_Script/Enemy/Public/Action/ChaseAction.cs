using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction<T> : BaseAction<T> where T : Enum
{
    Transform targetTrm;
    Action updateAction;

    int moveIdx;
    bool resetRoute;
    bool isMove;
    Vector3 nextPos;
    Vector3Int currentPos;
    List<Vector3Int> route;

    float beforeTime;
    float resetTime = 0.5f;

    public ChaseAction(BaseFSM_Controller<T> controller, Transform targetTrm, bool useNav) : base(controller)
    {
        this.targetTrm = GameManager.Instance.player;
        route = new();

        updateAction = useNav == true ? UseNavChase : NormalChase;
        isMove = true;
    }

    public override void OnEnter()
    {
        //Debug
        ResetRoute();
        controller.ChangeColor(Color.blue);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        updateAction.Invoke();
    }
    
    #region UseNav
    public void UseNavChase()
    {
        isMove = !controller.Nav.IsBaking;
        if (!isMove) return;
      
        if (resetRoute == true || (Time.time > beforeTime + resetTime && moveIdx >= route.Count))
            ResetRoute();

        Vector3 dir = nextPos - controller.transform.position;
        controller.transform.position += dir.normalized * _data.Speed * Time.deltaTime;
        if (dir.magnitude <= 0.05f)
        {
            SetNextTarget();
        }

        controller.PrintRoute(route);
    }

    private void SetNextTarget()
    {
        if (moveIdx >= route.Count)
        {
            resetRoute = true;
            return;
        }
        currentPos = route[moveIdx];
        nextPos = TilemapManager.Instance.GetWorldPos(currentPos);
        moveIdx++;
    }

    private void ResetRoute()
    {
        route = controller.Nav.UpdateNav(targetTrm.position);
        moveIdx = 0;
        resetRoute = false;
        beforeTime = Time.time;
    }
    #endregion

    private void NormalChase()
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
