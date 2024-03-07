using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolAction<T> : BaseAction<T> where T : Enum
{
    Vector2 startPos;
    Vector3 targetPos;
    float patrolRadius;
    bool idle;
    float idleTime;
    float idleTimer;
    float beforeIdleTime;
    float t;
    Vector2 beforeStopPos;
    Coroutine idleCor;

    Transform debuggingTrm;

    //Find Route
    int moveIdx;
    Vector3 nextPos;
    Vector3 currentPos;
    List<Vector3> route;
    Vector3 beforeDir;
    Vector3 curDir;

    List<SteeringBehaviour> behaviours;
    PatrolBehaviour patrolBehaviour;
    AIData aiData;

    public PatrolAction(BaseFSM_Controller<T> controller, List<SteeringBehaviour> behaviours, Transform debugTarget) : base(controller)
    {
        debuggingTrm = debugTarget;

        nextPos = controller.transform.position;
        patrolRadius = controller.EnemyDataSO.Range;
        aiData = controller.AIdata;
        idleTime = controller.EnemyDataSO.IdleTime;
        idleTimer = 1f;
        beforeIdleTime = Time.time;
        //idle = false;
        //idleCor = null;
        //route = new();
        //currentPos = controller.transform.position;
        //targetPos = currentPos;

        startPos = controller.transform.position;
        if (behaviours[0] is PatrolBehaviour)
        {
            patrolBehaviour = behaviours[0] as PatrolBehaviour;
            patrolBehaviour.Setting(startPos, Random.Range(10, 25));
        }
        else
        {
            //patrolBehaviour = new(ownerTrm: controller.transform, _data.Range);
            //behaviours.Add(patrolBehaviour);
            Debug.Log("error");
        }
        this.behaviours = behaviours;
    }

    private void GizmoDraw() => Gizmos.DrawWireSphere(startPos, _data.Range);

    public override void OnEnter()
    {
        StartIdleCor(idleTime);

        startPos = controller.transform.position;
        patrolBehaviour.Setting(startPos, Random.Range(10, 25));

        controller.FixedUpdateAction += OnFixedUpdate;
        controller.Enemy.enemyAnimController.SetMove(true);

        beforeIdleTime = Time.time;

        GizmoDrawer.Instance.Add(GizmoDraw);

        Debug.Log("Patrol");
    }

    public override void OnExit()
    {
        controller.FixedUpdateAction -= OnFixedUpdate;
        idle = false;
        StopCoroutine(idleCor);
        GizmoDrawer.Instance.Remove(GizmoDraw);
    }

    public override void OnUpdate()
    {
    }


    public override void OnFixedUpdate()
    {
        controller.Enemy.enemyAnimController.SetMove(!idle);
        if (idle)
        {
            return;
        }

        beforeDir = curDir;
        Vector2 movementInput = controller.Solver.GetDirectionToMove(behaviours, controller.AIdata);
        controller.Enemy.MovementInput = movementInput;
        curDir = movementInput;

        if (Vector2.Distance(controller.transform.position, startPos) > patrolRadius - 0.5f && t > idleTimer)
        {
            t = 0;
            //aiData.IsOutOfDistance = false;
            idle = true;
            controller.StopImmediately();
            beforeIdleTime = Time.time;
            StartIdleCor(idleTime);
        }

        t += Time.deltaTime;
        #region Route기반 Patrol
        //if (!controller.Nav.IsNavActive) return;
        //if (idle) return;

        //if (route == null || route.Count == 0)
        //{
        //    StartIdleCor(1f);
        //    return;
        //}

        //Vector2 dir = nextPos - controller.transform.position;
        //Vector2 position = _rigidbody.position + (dir.normalized * _data.Speed * Time.deltaTime);

        //if (dir.magnitude <= 0.05f)
        //{
        //    SetNextTarget();
        //}

        //controller.PrintRoute(route);
        ////Vector2 movementInput = controller.Solver.GetDirectionToMove( ,controller.AIdata);
        //controller.Enemy.MovementInput = dir.normalized;

        #endregion
    }

    private void SetNextTarget()
    {
        if (moveIdx >= route.Count)
        {
            //목적지에 도착
            route.Clear();
            idle = true;
            float idleTime = Random.Range(this.idleTime, this.idleTime + 0.5f);

            StartIdleCor(idleTime);

            return;
        }
        currentPos = route[moveIdx];
        nextPos = currentPos;
        moveIdx++;
    }

    private void SetToMove()
    {
        Debug.Log("SetTarget");
        //targetPos = FindRandomPoint(controller.transform.position);
        //SetTarget(targetPos);
        //route = controller.Nav.GetRoute(targetPos); //경로 검색
        //if (route != null && route.Count != 0)
        //    nextPos = route[0];

        //moveIdx = 0;
        idle = false;
    }
    private void StartIdleCor(float idleTime)
    {
        StopCoroutine(idleCor);
        idleCor = StartCoroutine(
              DelayCor
              (
                  idleTime,
                  afterAct:
                      () => SetToMove())
              );
    }
    private Vector2 FindRandomPoint(Vector2 pos)
    {
        Vector2 randomPos = controller.Nav.GetRandomPos(pos, patrolRadius);
        SetTarget(randomPos);
        return randomPos;
    }
    public void SetTarget(Vector2 target)
    {
        this.debuggingTrm.position = target;
    }
}
