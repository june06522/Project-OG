using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowyAttackState : GlowyRootState
{
    bool pointerOn;
    Vector2 endPos;

    public GlowyAttackState(GlowyStateController controller) : base(controller)
    {

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
        yield return new WaitForSeconds(0.5f);
        pointerOn = false;
        yield return new WaitForSeconds(0.1f);
        controller.SetLaserPointerActive(false);
        controller.Shoot(endPos);

        yield return new WaitForSeconds(0.5f);
        _data.SetCoolDown();
        controller.ChangeState(EGlowyState.Idle);
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {
        if(pointerOn)
        {
            Vector2 dir = controller.Target.position - controller.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(controller.attackPoint.position, dir.normalized, 50, LayerMask.GetMask("Wall"));
            if (hit == true)
            {
                controller.SetLaserPointer(hit.point);
                endPos = hit.point;
            }
            else
            {
                controller.SetLaserPointer(dir.normalized * 50);
                endPos = dir.normalized * 50;
            }
        }
    }
}
