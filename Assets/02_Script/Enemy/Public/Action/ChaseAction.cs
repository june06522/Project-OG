using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction<T> : BaseAction<T> where T : Enum
{
    bool following;

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

    List<SteeringBehaviour> behaviours;
    Vector2 movementInput;

    public ChaseAction(BaseFSM_Controller<T> controller, List<SteeringBehaviour> behaviours, bool checkCollision) : base(controller)
    {
        //this.targetTrm = targetTrm;
        this.behaviours = behaviours;
        //route = new();

        //updateAction = useNav == true ? UseNavChase : NormalChase;
        isMove = true;
    }

    public override void OnEnter()
    {
        //Debug
        Debug.Log("EnterChase");
        //ResetRoute();
        controller.ChangeColor(Color.blue);
        controller.FixedUpdateAction += OnFixedUpdate;  
    }

    public override void OnExit()
    {
        controller.StopImmediately();
        controller.FixedUpdateAction -= OnFixedUpdate;
    }

    public override void OnFixedUpdate()
    {
        Debug.Log("Gang");
        Vector2 movementInput = controller.Solver.GetDirectionToMove(behaviours, controller.AIdata);
        controller.Enemy.MovementInput = movementInput;
    }

    public override void OnUpdate()
    {
        
        //if (controller.AIdata.currentTarget != null)
        //{
        //    //Looking at the Target
        //    //OnPointerInput?.Invoke(controller.AIdata.currentTarget.position);
        //    if (following == false)
        //    {
        //        following = true;
        //        StartCoroutine(ChaseAndAttack());
        //    }
        //}
        //else if (aiData.GetTargetsCount() > 0)
        //{
        //    //Target acquisition logic
        //    aiData.currentTarget = aiData.targets[0];
        //}
        //updateAction.Invoke();
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
    //public void UseNavChase()
    //{
    //    Vector3 origin = controller.transform.position;
    //    Vector3 dir = (targetTrm.position - origin);
    //    dir.z = 0;

    //    Debug.DrawRay(origin, dir);

    //    Vector2 size = controller.Enemy.Collider.bounds.size;
    //    float angle = Vector3.Angle(targetTrm.position, origin);
    //    RaycastHit2D hit = Physics2D.BoxCast(origin, size, 
    //                                    angle, dir.normalized, dir.magnitude, LayerMask.GetMask("Wall", "Obstacle"));
    //    if (hit)
    //    {
    //        Debug.Log("NavChase");
    //        NavAction();
    //        useNav = true;
    //    }
    //    else
    //    {
    //        if(useNav == true)
    //            ResetRoute();

    //        Debug.Log("NormalChase");
    //        useNav = false;
    //        NormalChase();
    //    }
    //}

    //private void NavAction()
    //{

    //    if (useNav == false)
    //        ResetRoute();

    //    if (route == null || route.Count == 0)
    //    {
    //        ResetRoute();
    //        return;
    //    }

    //    Vector2 dir = nextPos - controller.transform.position;
  
    //    Vector3 position = controller.Enemy.Rigidbody.position
    //                         + (dir.normalized * _data.Speed * Time.deltaTime);

    //    controller.Enemy.Rigidbody.MovePosition(position);

    //    if (dir.magnitude <= 0.05f)
    //    {
    //        SetNextTarget();
    //    }

    //    controller.PrintRoute(route);
    //    controller.Flip(dir.x < 0);
    //}

    //private void SetNextTarget()
    //{

    //    if (moveIdx >= route.Count)
    //    {
    //        ResetRoute();
    //        isArrived = true;
    //        return;
    //    }
    //    currentPos = route[moveIdx];
    //    nextPos = currentPos;
    //    moveIdx++;
    //}

    //private void ResetRoute()
    //{
    //    currentPos = controller.transform.position;
    //    nextPos = currentPos;
    //    route = controller.Nav.GetRoute(targetTrm.position);
    //    moveIdx = 0;
    //    isArrived = false;
    //    beforeTime = Time.time;
    //}

    #endregion

}
