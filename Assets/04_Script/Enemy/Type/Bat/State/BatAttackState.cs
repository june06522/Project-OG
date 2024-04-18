
using DG.Tweening;
using FSM_System;
using System;
using System.Collections;
using UnityEngine;

public class BatAttackState : FSM_State<ENormalEnemyState>
{
    Transform targetTrm;
    private new BatStateController controller;

    public BatAttackState(BatStateController controller) : base(controller)
    {
        this.controller = controller;
        targetTrm = GameManager.Instance.player.transform;
    }

    protected override void EnterState()
    {
        controller.Enemy.enemyAnimController.SetAttack();
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

        controller.EnemyDataSO.SetCoolDown();
        controller.ChangeState(ENormalEnemyState.Move);
    }

    private void CheckHit()
    {
        Collider2D col = Physics2D.OverlapCircle(controller.attackPoint.position, 0.25f, LayerMask.GetMask("Player"));
        if (col)
        {
            IHitAble hitAble;
            if (col.TryGetComponent<IHitAble>(out hitAble))
            {
                hitAble.Hit(controller.EnemyDataSO.AttackPower);
            }
            else
                Debug.Log(col.gameObject.name);
        }
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

    }
}
