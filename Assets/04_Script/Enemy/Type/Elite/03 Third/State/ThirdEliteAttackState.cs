using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;
using DG.Tweening;

public class ThirdEliteAttackState : FSM_State<ENormalPatrolEnemyState>
{
    private ThirdEliteStateController _controller;

    public ThirdEliteAttackState(ThirdEliteStateController controller) : base(controller)
    {
        _controller = controller;
    }

    protected override void EnterState()
    {
        StartCoroutine(Skill(10, 0.1f, 5, 60));
    }

    protected override void ExitState()
    {
        
    }

    protected override void UpdateState()
    {
        
    }

    private IEnumerator Skill(int bulletCount, float waitTime, float speed, float angle)
    {
        Vector2 dir = (GameManager.Instance.player.transform.position - _controller.transform.position).normalized;
        Vector3 originSize = _controller.transform.localScale;

        for(int i = 0; i < bulletCount; i++)
        {
            _controller.transform.DOScale(originSize * 1.2f, 0.1f)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _controller.transform.DOScale(originSize, 0.1f)
                        .SetEase(Ease.InOutSine);
                });

            yield return new WaitForSeconds(0.1f);

            GameObject bullet = UnityEngine.Object.Instantiate(_controller.bullet);
            bullet.transform.position = _controller.transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 temp = Quaternion.Euler(0, 0, Random.Range(-angle / 2, angle / 2)) * dir;
            rigid.velocity = temp.normalized * speed;

            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(0.5f);
        _controller.EnemyDataSO.SetCoolDown();
        _controller.ChangeState(ENormalPatrolEnemyState.Idle);
    }
}
