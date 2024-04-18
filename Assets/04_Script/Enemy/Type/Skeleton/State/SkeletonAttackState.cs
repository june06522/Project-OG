using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : NormalRootState
{
    Transform _targetTrm;
    Transform _attackPoint;
    Transform _weapon;

    public SkeletonAttackState(BaseFSM_Controller<ENormalEnemyState> controller, Transform attackPoint, Transform weapon) : base(controller)
    {
        _targetTrm = GameManager.Instance.player.transform;
        _attackPoint = attackPoint;
        _weapon = weapon;
    }

    protected override void EnterState()
    {
        Attack();
    }

    private void Attack()
    {
        CheckHit();
        controller.transform.DOMove(_targetTrm.position, 0.25f).SetEase(Ease.InSine).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            StartCoroutine(AttackEndEvt());
        });

        Sequence seq = DOTween.Sequence();
        seq.Append(_weapon.DORotate(new Vector3(0f, 0f, 120f), 0.25f).SetEase(Ease.InElastic))
            .Append(_weapon.DORotate(new Vector3(0, 0, 0f), 0.1f).SetEase(Ease.InBack));
        
    }

    private IEnumerator AttackEndEvt()
    {
        yield return new WaitForSeconds(0.3f);

        _data.SetCoolDown();
        controller.ChangeState(ENormalEnemyState.Move);
    }

    private void CheckHit()
    {
        Collider2D col = Physics2D.OverlapCircle(_attackPoint.position, 0.45f, LayerMask.GetMask("Player"));
        if (col)
        {
            IHitAble hitAble;
            if (col.TryGetComponent<IHitAble>(out hitAble))
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
        Vector2 dir = _targetTrm.position - controller.transform.position;
        controller.Enemy.enemyAnimController.Flip(dir);
    }
}
