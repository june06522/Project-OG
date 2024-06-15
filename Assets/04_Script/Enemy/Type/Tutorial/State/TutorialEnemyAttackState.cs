using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyAttackState : FSM_State<ENormalPatrolEnemyState>
{
    bool pointerOn;
    Vector2 endPos;
    Vector2 beforeEndPos;

    new TutorialEnemyStateController controller;

    public TutorialEnemyAttackState(TutorialEnemyStateController controller) : base(controller)
    {
        this.controller = controller;
    }

    public void AttackState() => EnterState();

    protected override void EnterState()
    {
        controller.StopImmediately();

        pointerOn = true;

        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
       
        controller.Shoot();


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
