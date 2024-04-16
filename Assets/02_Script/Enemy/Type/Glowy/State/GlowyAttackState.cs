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
        bool attackEnd = false;

        controller.SetLaserPointerActive(true);

        while(attackEnd == false)
        {
            
            yield return null;
        }
        bool fakeMotion = UnityEngine.Random.Range(0, 2) == 0;

        yield return new WaitForSeconds(0.5f);

        
        
        pointerOn = false;
        yield return new WaitForSeconds(0.1f);
        if (fakeMotion)
        {
            yield return new WaitForSeconds(0.15f);
            pointerOn = true;
            yield return new WaitForSeconds(0.2f);
        }
        controller.SetLaserPointerActive(false);
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
       
    }
}
