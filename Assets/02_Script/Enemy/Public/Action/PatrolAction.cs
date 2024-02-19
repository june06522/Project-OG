using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolAction<T> : BaseAction<T> where T : Enum
{
    Vector3 targetPos;
    float patrolRadius;
    bool idle;
    float idleTime;

    Coroutine idleCor;

    Transform debuggingTrm;

    //Find Route
    int moveIdx;
    Vector3 nextPos;
    Vector3Int currentPos;
    List<Vector3Int> route;

    public PatrolAction(BaseFSM_Controller<T> controller, Transform debugTarget) : base(controller)
    {
        debuggingTrm = debugTarget;
        targetPos = Vector2.zero;
        patrolRadius = _data.Range;
        idle = false;
        idleTime = controller.EnemyData.IdleTime;
        idleCor = null;
        route = new();
    }

    public override void OnEnter()
    {
        idle = true;
        moveIdx = 0;
        route.Clear();
        StartIdleCor(idleTime);
  
        controller.ChangeColor(Color.white);
    }

    public override void OnExit()
    {
        idle = false;
        StopCoroutine(idleCor);
    }

    public override void OnUpdate()
    {
        if (idle) return;

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
            //목적지에 도착

            idle = true;
            float idleTime = Random.Range(this.idleTime - 0.3f, this.idleTime + 0.3f);

            StartIdleCor(idleTime);

            return;
        }
        currentPos = route[moveIdx];
        nextPos = TilemapManager.Instance.GetWorldPos(currentPos);
        moveIdx++;
    }

    private void SetToMove()
    {
        targetPos = FindRandomPoint(controller.transform.position);
        route = controller.Nav.UpdateNav(targetPos); //경로 검색
        moveIdx = 0;
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
        Vector2 randomPos = controller.Nav.GetRandomPos();
        SetTarget(randomPos);
        return randomPos;
    }
    public void SetTarget(Vector2 target)
    {
        this.debuggingTrm.position = target;
    }

}
