
using DG.Tweening;
using System;
using UnityEngine;

public class BatAttackState : BatEnemyRootState
{
    Transform targetTrm;
    BatFSMController con;

    public BatAttackState(BatFSMController controller) : base(controller)
    {
        targetTrm = GameManager.Instance.player.transform;
        con = controller;
    }

    protected override void EnterState()
    {
        con.ChangeColor(Color.red);
        Attack();
    }

    private void Attack()
    {
        CheckHit();
        controller.transform.DOMove(targetTrm.position, 0.25f).SetEase(Ease.InSine).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            AttackEndEvt();
        });
    }

    private void AttackEndEvt()
    {
        DataSO.SetCoolDown();
        controller.ChangeState(EBatState.Chase);
    }

    private void CheckHit()
    {
        Collider2D col = Physics2D.OverlapCircle(con.attackPoint.position, 0.25f, LayerMask.GetMask("Player"));
        if (col)
        {
            col.GetComponent<IHitAble>().Hit(DataSO.AttackPower);
        }
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

    }
}
