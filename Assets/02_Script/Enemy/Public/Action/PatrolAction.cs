using System;
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

    public PatrolAction(BaseFSM_Controller<T> controller, Transform debugTarget) : base(controller)
    {
        debuggingTrm = debugTarget;
        targetPos = Vector2.zero;
        patrolRadius = _data.Range;
        idle = false;
        idleTime = controller.EnemyData.IdleTime;
        idleCor = null;
    }

    public override void OnEnter()
    {
        idle = true;
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

        Vector3 dir = (targetPos - controller.transform.position).normalized;
        controller.transform.position += dir * _data.Speed * Time.deltaTime;

        //목적지에 도착
        if (Vector3.Distance(targetPos, controller.transform.position) < 0.05f)
        {
            idle = true;
            float idleTime = Random.Range(this.idleTime - 0.3f, this.idleTime + 0.3f);

            StartIdleCor(idleTime);
        }
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
        Vector2 randomPointOnCircle = UnityEngine.Random.insideUnitSphere;
        randomPointOnCircle.Normalize();
        randomPointOnCircle *= patrolRadius;

        Vector2 targetTrm = randomPointOnCircle + pos;
        Vector2 dir = (targetTrm - pos).normalized;


        RaycastHit2D hit = Physics2D.Raycast(pos, dir, patrolRadius, _data.ObstacleLayer);

        if (hit.collider != null)
            targetTrm = hit.point - dir;

        SetTarget(targetTrm);
        return targetTrm;
    }

    private void SetToMove()
    {
        targetPos = FindRandomPoint(controller.transform.position);
        idle = false;
    }

    public void SetTarget(Vector2 target)
    {
        this.debuggingTrm.position = target;
    }
}
