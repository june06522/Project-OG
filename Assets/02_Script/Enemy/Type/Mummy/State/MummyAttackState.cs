using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyAttackState : MummyRootState
{
    Transform targetTrm;
    Transform _attackPoint;

    public MummyAttackState(MummyStateController controller) : base(controller)
    {
        targetTrm = GameManager.Instance.player.transform;
        _attackPoint = controller.attackPoint;
    }

    protected override void EnterState()
    {
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
        controller.ChangeState(EMummyState.Move);
    }

    private void CheckHit()
    {
        Collider2D col = Physics2D.OverlapCircle(_attackPoint.position, 0.4f, LayerMask.GetMask("Player"));
        if (col)
        {
            IHitAble hitAble;
            if(col.TryGetComponent<IHitAble>(out hitAble))
            {
                hitAble.Hit(_data.AttackPower);
            }
        }
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {
        Vector2 dir = targetTrm.position - controller.transform.position;
        controller.Enemy.enemyAnimController.Flip(dir);
    }
}
