using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;
using DG.Tweening;

public class FifthEliteAttackState : FSM_State<ENormalEnemyState>
{
    private FifthEliteStateController _controller;

    public FifthEliteAttackState(FifthEliteStateController controller) : base(controller)
    {
        _controller = controller;
    }

    protected override void EnterState()
    {
        Attack();
    }

    protected override void ExitState()
    {
        
    }

    protected override void UpdateState()
    {
        
    }

    private void Attack()
    {
        CheckHit();
        controller.transform.DOMove(GameManager.Instance.player.transform.position, 0.25f).SetEase(Ease.InSine).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            StartCoroutine(AttackEndEvt());
        });
    }

    private IEnumerator AttackEndEvt()
    {
        yield return new WaitForSeconds(0.3f);

        _controller.EnemyDataSO.SetCoolDown();
        _controller.ChangeState(ENormalEnemyState.Move);
    }

    private void CheckHit()
    {
        Collider2D col = Physics2D.OverlapCircle(controller.transform.position, 0.25f, LayerMask.GetMask("Player"));
        if (col)
        {
            IHitAble hitAble;
            if (col.TryGetComponent<IHitAble>(out hitAble))
            {
                hitAble.Hit(_controller.EnemyDataSO.AttackPower);
            }
            else
                Debug.Log(col.gameObject.name);
        }
    }
}
