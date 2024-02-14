using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyGunAttackState : MummyGunRootState
{
    Transform targetTrm;
    public MummyGunAttackState(MummyGunStateController controller) : base(controller)
    {
        targetTrm = GameManager.Instance.player.transform;
    }

    protected override void EnterState()
    {
        controller.ChangeColor(Color.red);
        
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.3f);

        Sequence seq = DOTween.Sequence();
        Tween scaleTween =
            controller.transform.DOScale(Vector3.one * 2, 0.5f).SetEase(Ease.InSine);
        Tween shakeTween =
            controller.transform.DOShakeScale(0.2f);
        Tween rescaleTween =
            controller.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InSine);

        seq.Append(scaleTween);
        seq.Append(shakeTween).AppendCallback(()=> Shoot());
        seq.Append(rescaleTween);
        seq.OnComplete(() =>
        {
            StartCoroutine(AttackEndEvt());
        });
        seq.Play();
    }

    private void Shoot()
    {
        Vector2 dir =  (targetTrm.position - controller.attackPoint.position).normalized;
        controller.InstantiateBullet(dir, EEnemyBulletSpeedType.Linear, EEnemyBulletCurveType.None);
    }

    private IEnumerator AttackEndEvt()
    {
        yield return new WaitForSeconds(0.3f);

        _data.SetCoolDown();
        controller.ChangeState(EMummyGunState.Idle);
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

    }
}
