using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;

public class NipperDashEnemyAttackState : FSM_State<ENormalEnemyState>
{
    private NipperDashEnemyStateController _controller;

    public NipperDashEnemyAttackState(NipperDashEnemyStateController controller) : base(controller)
    {
        _controller = controller;
    }

    protected override void EnterState()
    {
        StartCoroutine(Dash(0.2f, 0.5f));
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

    }

    private IEnumerator Dash(float dashTime, float slideTime)
    {
        Vector3 targetPos = GameManager.Instance.player.position;

        float dis = Vector3.Distance(GameManager.Instance.player.position, transform.position);
        float speed = dis / dashTime;
        float friction = 0.96f;
        float curTime = 0;

        Vector2 dir = GameManager.Instance.player.position - transform.position;
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, z - 90);

        _controller.animator.SetTrigger(_controller.dehisce);
        yield return new WaitForSeconds(0.5f);

        _controller.animator.SetTrigger(_controller.shut);

        while(curTime < dashTime && !_controller.isStop)
        {
            curTime += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
            speed *= friction;
            yield return null;
        }

        curTime = 0;
        while(curTime < slideTime && !_controller.isStop)
        {
            curTime += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + (Vector3)dir, Time.deltaTime * speed);
            speed *= friction;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        _controller.animator.SetTrigger(_controller.idle);
        _controller.EnemyDataSO.SetCoolDown();
        _controller.ChangeState(ENormalEnemyState.Idle);
        _controller.isStop = false;
    }
}
