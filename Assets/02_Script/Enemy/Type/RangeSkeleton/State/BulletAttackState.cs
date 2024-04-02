using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttackState : MummyRootState
{
    private Transform _target;
    private EnemyBullet _bullet;
    private Transform _gun;

    public BulletAttackState(BaseFSM_Controller<EMummyState> controller, EnemyBullet bullet, Transform gun = null) : base(controller)
    {
        _target = GameManager.Instance.player;
        _bullet = bullet;
        _gun = gun;
    }

    protected override void EnterState()
    {
        Attack();
    }

    private void Attack()
    {
        // ÃÑ¾Ë ¹ß»ç
        controller.Enemy.enemyAnimController.SetMove(false);

        Sequence seq = DOTween.Sequence();
        seq.Append(_gun.DORotate(new Vector3(0f, 0f, -30f), 0.25f).SetEase(Ease.InElastic))
            .AppendCallback(() =>
            {
                EnemyBullet spawnBullet = GameObject.Instantiate(_bullet, _gun.position, Quaternion.identity);
                spawnBullet.Shoot(_target.transform.position - _gun.transform.position, EEnemyBulletSpeedType.Linear);
                StartCoroutine(AttackEndEvt());
            });

        seq.Append(_gun.DORotate(new Vector3(0, 0, 0f), 0.3f).SetEase(Ease.InBack));

        
    }

    private IEnumerator AttackEndEvt()
    {
        yield return new WaitForSeconds(0.3f);

        _data.SetCoolDown();
        controller.ChangeState(EMummyState.Idle);
    }

    protected override void ExitState()
    {

    }

}
