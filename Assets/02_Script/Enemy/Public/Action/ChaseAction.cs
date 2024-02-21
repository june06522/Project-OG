using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction<T> : BaseAction<T> where T : Enum
{
    Transform targetTrm;
    Action updateAction;

    int moveIdx;
    bool isArrived;
    bool isMove;

    Vector3 nextPos;
    Vector3Int currentPos;
    List<Vector3Int> route;

    float beforeTime;
    float resetTime = 0.5f;

    public ChaseAction(BaseFSM_Controller<T> controller, Transform targetTrm, bool useNav) : base(controller)
    {
        this.targetTrm = GameManager.Instance.player;
        //route = new();

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
        
        //º® °Ë»ç


    }

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

    #region UseNav
    public void UseNavChase()
    {
        Vector3 origin = controller.transform.position;
        Vector3 dir = (targetTrm.position - origin);
        dir.z = 0;

        Debug.DrawRay(origin, dir);

        Vector2 size = controller.Enemy.Collider.bounds.size;
        float radius = Math.Max(size.x, size.y) * 0.5f;
        RaycastHit2D hit = Physics2D.CircleCast(origin, radius,
                                        dir.normalized, dir.magnitude, LayerMask.GetMask("Wall"));
        if (!hit)
        {
          
            Debug.Log("NavChase");
            NavAction();
        }
        else
        {
            Debug.Log("NormalChase");
            NormalChase();
        }
    }

    private void NavAction()
    {

        if (route == null || route.Count == 0)
        {
            ResetRoute();
            return;
        }

        Debug.Log("Chasing");

        Vector3 dir = nextPos - controller.transform.position;
        controller.transform.position += dir.normalized * _data.Speed * Time.deltaTime;
        if (dir.magnitude <= 0.05f)
        {
            SetNextTarget();
        }

        controller.PrintRoute(route);
        controller.Flip(dir.x < 0);
    }

    private void SetNextTarget()
    {

        if (moveIdx >= route.Count / 2)
        {
            ResetRoute();
            isArrived = true;
            return;
        }
        currentPos = route[moveIdx];
        nextPos = TilemapManager.Instance.GetWorldPos(currentPos);
        moveIdx++;
    }

    private void ResetRoute()
    {
        //currentPos = TilemapManager.Instance.GetTilePos(controller.transform.position);
        //nextPos = TilemapManager.Instance.GetWorldPos(currentPos);
        route = controller.Nav.GetRoute(targetTrm.position);
        moveIdx = 0;
        isArrived = false;
        beforeTime = Time.time;
    }

    #endregion

}
