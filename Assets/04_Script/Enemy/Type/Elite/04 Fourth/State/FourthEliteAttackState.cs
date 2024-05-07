using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;
using DG.Tweening;

public class FourthEliteAttackState : FSM_State<ENormalPatrolEnemyState>
{
    private FourthEliteStateController _controller;
    public FourthEliteAttackState(FourthEliteStateController controller) : base(controller)
    {
        _controller = controller;
    }

    protected override void EnterState()
    {
        StartCoroutine(Skill(10, 10));
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

    }

    private IEnumerator Skill(int bulletCount, float speed)
    {
        Vector3 originSize = _controller.transform.localScale;

        _controller.transform.DOScale(originSize * 1.2f, 0.1f)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _controller.transform.DOScale(originSize, 0.1f)
                        .SetEase(Ease.InOutSine);
                });

        for(int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = UnityEngine.Object.Instantiate(_controller.bullet);
            bullet.transform.position = _controller.transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bulletCount), Mathf.Sin(Mathf.PI * 2 * i / bulletCount));
            rigid.velocity = dir.normalized * speed;
        }

        _controller.EnemyDataSO.SetCoolDown();
        _controller.ChangeState(ENormalPatrolEnemyState.Idle);

        yield return null;
    }
}
