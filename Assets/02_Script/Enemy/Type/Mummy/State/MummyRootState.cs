using FSM_System;
using System.Collections;
using UnityEngine;

//root°â patrolState
public class MummyRootState : FSM_State<EMummyState>
{
    protected new MummyStateController controller;
    protected EnemyDataSO _data => controller.EnemyData;
    
    Vector3 targetPos = Vector3.zero;
    float patrolRadius;
    bool idle = false;
    float idleTime = 1f;

    Coroutine idleCor = null;

    public MummyRootState(MummyStateController controller) : base(controller)
    {
        this.controller = controller;
        patrolRadius = _data.Range;
    }


    protected override void EnterState()
    {
        idle = true;
        controller.ChangeColor(Color.white);
        idleCor = StartCoroutine(IdleCor(idleTime));
    }

    protected override void ExitState()
    {
        idle = false;
        if(idleCor != null)
            StopCoroutine(idleCor);
    }

    protected override void UpdateState()
    {
        if (idle) return;

        Vector3 dir = (targetPos - controller.transform.position).normalized;
        controller.transform.position += dir * _data.Speed * Time.deltaTime;

        //¸ñÀûÁö¿¡ µµÂø
        if(Vector3.Distance(targetPos, controller.transform.position) < 0.05f)
        {
            idle = true;
            float idleTime = Random.Range(this.idleTime - 0.3f, this.idleTime + 0.3f);
            if (idleCor != null)
                StopCoroutine(idleCor);
            idleCor = StartCoroutine(IdleCor(idleTime));
        }
    }

    private Vector2 FindRandomPoint(Vector2 pos)
    {
        Vector2 randomPointOnCircle = Random.insideUnitSphere;
        randomPointOnCircle.Normalize();
        randomPointOnCircle *= patrolRadius;

        Vector2 targetTrm = randomPointOnCircle + pos;
        Vector2 dir = (targetTrm - pos).normalized;


        RaycastHit2D hit = Physics2D.Raycast(pos, dir, patrolRadius, _data.ObstacleLayer);

        if (hit.collider != null)
            targetTrm = hit.point - dir;

        controller.SetTarget(targetTrm);
        return targetTrm;
    }

    private IEnumerator IdleCor(float idleTime)
    {
        yield return new WaitForSeconds(idleTime);
        targetPos = FindRandomPoint(controller.transform.position);
        idle = false;
    }
}
    