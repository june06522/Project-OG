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

    bool useNav;

    Vector3 nextPos;
    Vector3 currentPos;
    List<Vector3> route;

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
        Debug.Log("EnterChase");
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
        Vector2 dir = (targetTrm.position - controller.transform.position);
        float speed = _data.Speed;

        if (dir.magnitude > _data.AttackAbleRange)
        {
            Vector3 position = controller.Enemy.Rigidbody.position 
                                + (dir.normalized * speed * Time.deltaTime);
            controller.Enemy.Rigidbody.MovePosition(position);
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
                                        dir.normalized, dir.magnitude, LayerMask.GetMask("Wall", "Obstacle"));
        if (hit)
        {
            Debug.Log("NavChase");
            NavAction();
            useNav = true;
        }
        else
        {
            if(useNav == true)
                ResetRoute();

            Debug.Log("NormalChase");
            useNav = false;
            NormalChase();
        }
    }

    private void NavAction()
    {

        if (useNav == false)
            ResetRoute();

        if (route == null || route.Count == 0)
        {
            ResetRoute();
            return;
        }

        Vector2 dir = nextPos - controller.transform.position;
  
        Vector3 position = controller.Enemy.Rigidbody.position
                             + (dir.normalized * _data.Speed * Time.deltaTime);

        controller.Enemy.Rigidbody.MovePosition(position);

        if (dir.magnitude <= 0.05f)
        {
            SetNextTarget();
        }

        controller.PrintRoute(route);
        controller.Flip(dir.x < 0);
    }

    private void SetNextTarget()
    {

        if (moveIdx >= route.Count)
        {
            ResetRoute();
            isArrived = true;
            return;
        }
        currentPos = route[moveIdx];
        nextPos = currentPos;
        moveIdx++;
    }

    private void ResetRoute()
    {
        currentPos = controller.transform.position;
        nextPos = currentPos;
        route = controller.Nav.GetRoute(targetTrm.position);
        moveIdx = 0;
        isArrived = false;
        beforeTime = Time.time;
    }

    #endregion

}
