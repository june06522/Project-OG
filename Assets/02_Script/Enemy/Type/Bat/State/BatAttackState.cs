
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BatAttackState : BatEnemyRootState
{
    Transform targetTrm;

    public BatAttackState(BatStateController controller) : base(controller)
    {
        targetTrm = GameManager.Instance.player.transform;
    }

    protected override void EnterState()
    {
        controller.ChangeColor(Color.red);
        Attack();
    }

    private void Attack()
    {
        CheckHit();
        controller.transform.DOMove(targetTrm.position, 0.25f).SetEase(Ease.InSine).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            StartCoroutine(AttackEndEvt());
        });
    }

    private IEnumerator AttackEndEvt()
    {
        yield return new WaitForSeconds(0.3f);

        _data.SetCoolDown();
        controller.ChangeState(EBatState.Move);
    }

    private void CheckHit()
    {
        Collider2D col = Physics2D.OverlapCircle(controller.attackPoint.position, 0.25f, LayerMask.GetMask("Player"));
        if (col)
        {
            col.GetComponent<IHitAble>().Hit(_data.AttackPower);
        }
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

    }
}
