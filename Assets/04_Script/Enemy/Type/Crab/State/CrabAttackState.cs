using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrabAttackType
{
    Laser,
    Rush,
    End
}


public class CrabAttackState : NormalPatrolRootState
{
    private new CrabStateController controller;
    public CrabAttackState(BaseFSM_Controller<ENormalPatrolEnemyState> controller) : base(controller)
    {
        this.controller = controller as CrabStateController; 
    }

    protected override void EnterState()
    {
        controller.StopImmediately();
        
        Attack();
    }

    private void Attack()
    {
        CrabAttackType ranodmAttack 
            = (CrabAttackType)Random.Range(0, (int)CrabAttackType.End);
        switch (ranodmAttack)
        {
            case CrabAttackType.Laser:
                LaserAttack();
                break;
            case CrabAttackType.Rush:
                RushAttack();
                break;
        }
    }

    private void RushAttack()
    {
        
    }

    private void LaserAttack()
    {
        controller.LaserAttackPoints.ForEach((attackTrm) =>
        {
            //attackTrm.position
        });
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

    }
}
