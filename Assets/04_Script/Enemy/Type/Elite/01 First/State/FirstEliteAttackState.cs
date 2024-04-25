using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;
using DG.Tweening;

public class FirstEliteAttackState : FSM_State<ENormalPatrolEnemyState>
{
    private FirstEliteStateController _controller;

    public FirstEliteAttackState(FirstEliteStateController controller) : base(controller)
    {
        _controller = controller;
    }

    protected override void EnterState()
    {
        StartCoroutine(Skill(3, 5, 10));
    }

    protected override void ExitState()
    {
        
    }

    protected override void UpdateState()
    {
        
    }

    private IEnumerator Skill(int bulletCount, float angle, float speed)
    {
        int bc = 0;
        if (bulletCount % 2 == 0)
        {
            bc = bulletCount / 2;
        }
        else
        {
            bc = bulletCount / 2 + 1;
        }
        Vector2 dir = (GameManager.Instance.player.transform.position - _controller.transform.position).normalized;
        Vector3 originSize = _controller.transform.localScale;

        _controller.transform.DOScale(originSize * 1.2f, 0.1f)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _controller.transform.DOScale(originSize, 0.1f)
                        .SetEase(Ease.InOutSine);
                });

        yield return new WaitForSeconds(0.1f);

        for (int i = -(bulletCount / 2); i < bc; i++)
        {
            GameObject bullet = UnityEngine.Object.Instantiate(_controller.bullet);
            bullet.transform.position = _controller.transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 temp = Quaternion.Euler(0, 0, i * angle) * dir;
            rigid.velocity = temp.normalized * speed;
        }

        _controller.EnemyDataSO.SetCoolDown();
        _controller.ChangeState(ENormalPatrolEnemyState.Idle);

        yield return null;
    }
}
