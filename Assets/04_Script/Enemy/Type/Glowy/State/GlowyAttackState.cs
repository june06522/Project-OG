using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowyAttackState : FSM_State<ENormalPatrolEnemyState>
{
    bool pointerOn;
    Vector2 endPos;
    Vector2 beforeEndPos;

    new GlowyStateController controller;

    public GlowyAttackState(GlowyStateController controller) : base(controller)
    {
        this.controller = controller;
    }

    protected override void EnterState()
    {
        controller.StopImmediately();

        pointerOn = true;
        controller.SetLaserPointerActive(true);

        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        controller.ResetPoints();
        controller.SetLaserPointerActive(false);
        yield return new WaitForSeconds(0.5f);

        bool fakeMotion = UnityEngine.Random.Range(0, 2) == 0;

        float angle = 70;
        Vector3 dir;// = (controller.Target.position - controller.transform.position).normalized;
        Vector3 leftdir;// = (Quaternion.Euler(new Vector3(0,0, -angle)) * dir);
        Vector3 rightdir;// = (Quaternion.Euler(new Vector3(0, 0, angle)) * dir);

        dir = (controller.Target.position - controller.transform.position).normalized;
        leftdir = (Quaternion.Euler(new Vector3(0, 0, -angle)) * dir);
        rightdir = (Quaternion.Euler(new Vector3(0, 0, angle)) * dir);

        leftdir.z = 0;
        rightdir.z = 0f;
       
        do
        {

            RaycastHit2D lefthit = Physics2D.Raycast(controller.attackPoint.position, leftdir.normalized, 50, LayerMask.GetMask("Wall"));
            if (lefthit == true)
            {
                controller.SetLaserPointer(lefthit.point, 0);
            }
            else
            {
                controller.SetLaserPointer(leftdir.normalized * 50, 0);
            }

            RaycastHit2D rightHit = Physics2D.Raycast(controller.attackPoint.position, rightdir.normalized, 50, LayerMask.GetMask("Wall"));
            if (rightHit == true)
            {
                controller.SetLaserPointer(rightHit.point, 1);
            }
            else
            {
                controller.SetLaserPointer(rightdir.normalized * 50, 1);
            }

            controller.SetLaserPointerActive(true);

            dir = (controller.Target.position - controller.transform.position).normalized;
            leftdir = Vector3.Slerp(leftdir, dir, 0.3f);
            rightdir = Vector3.Slerp(rightdir, dir, 0.3f);

           angle -= Mathf.LerpAngle(angle, 0, 0.3f);
            yield return new WaitForSeconds(0.05f);
        }
        while (angle > 0.05f);

        dir = Vector3.Slerp(leftdir, rightdir, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(controller.attackPoint.position, dir.normalized, 50, LayerMask.GetMask("Wall"));
        if (hit == true)
        {
            endPos = hit.point;
        }
        else
        {
            endPos = dir * 50f;
        }

        pointerOn = false;
        //yield return new WaitForSeconds(0.1f);
        //if (fakeMotion)
        //{
        //    yield return new WaitForSeconds(0.15f);
        //    pointerOn = true;
        //    yield return new WaitForSeconds(0.2f);
        //}

        controller.SetLaserPointer(endPos, 0);
        controller.SetLaserPointer(endPos, 1);
        yield return new WaitForSeconds(0.1f);
        controller.SetLaserPointerActive(false);

        yield return new WaitForSeconds(0.1f);
        controller.Shoot(endPos);


        yield return new WaitForSeconds(0.5f);
        controller.EnemyDataSO.SetCoolDown();
        controller.ChangeState(ENormalPatrolEnemyState.Idle);
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {
        if (pointerOn)
        {
           
        }
    }
}
