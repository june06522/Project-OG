using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafFairyAttackState : LeafFairyRootState
{
    Transform targetTrm;
    public LeafFairyAttackState(LeafFairyStateController controller) : base(controller)
    {
        targetTrm = controller.Target;
    }

    protected override void EnterState()
    {
        //controller.Enemy.enemyAnimController.SetAttack();
        
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.3f);

        StartCoroutine(controller.Attack(() => StartCoroutine(AttackEndEvt())));
    }

    private IEnumerator AttackEndEvt()
    {
        yield return new WaitForSeconds(0.3f);

        _data.SetCoolDown();
        controller.ChangeState(ELeafFariyState.Idle);
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

    }
}
