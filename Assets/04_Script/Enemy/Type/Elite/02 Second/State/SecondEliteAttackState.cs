using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;
using DG.Tweening;

public class SecondEliteAttackState : FSM_State<ENormalPatrolEnemyState>
{
    private SecondEliteStateController _controller;

    public SecondEliteAttackState(SecondEliteStateController controller) : base(controller)
    {
        _controller = controller;
    }

    protected override void EnterState()
    {
        StartCoroutine(Skill(10, 1.5f, 5));
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

    }

    private IEnumerator Skill(int splitBullet, float waitTime, float speed)
    {
        GameObject[] bullets = new GameObject[splitBullet];
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

        GameObject bullet = UnityEngine.Object.Instantiate(_controller.bullet);
        bullet.transform.position = _controller.transform.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = dir * speed;

        Vector3 bulletOriginSize = bullet.transform.localScale;

        yield return new WaitForSeconds(waitTime);
        Transform trans = bullet.transform;
        UnityEngine.Object.Destroy(bullet);

        for (int i = 0; i < splitBullet; i++)
        {
            bullets[i] = UnityEngine.Object.Instantiate(_controller.bullet);
            bullets[i].transform.position = trans.position;
            bullets[i].transform.rotation = Quaternion.identity;
            bullets[i].transform.localScale = bulletOriginSize / 2;

            Rigidbody2D bulletsRigid = bullets[i].GetComponent<Rigidbody2D>();
            Vector2 temp = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / splitBullet), Mathf.Sin(Mathf.PI * 2 * i / splitBullet));
            bulletsRigid.velocity = temp.normalized * speed * 2;
        }

        yield return new WaitForSeconds(0.5f);
        _controller.EnemyDataSO.SetCoolDown();
        _controller.ChangeState(ENormalPatrolEnemyState.Idle);
    }
}
