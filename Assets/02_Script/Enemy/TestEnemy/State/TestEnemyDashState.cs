using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestEnemyDashState : TestEnemyRootState
{
    TestEnemyDataSO data;
    Transform targetTrm;

    TestEnemyFSMController con;
    public TestEnemyDashState(TestEnemyFSMController controller) : base(controller)
    {
        data = controller.EnemyData;
        targetTrm = GameObject.Find("Player").transform;
        con = controller;
    }

    protected override void EnterState()
    {
        Dash();
    }

    private void Dash()
    {
        Vector2 targetPos = targetTrm.position;

        float distance = Mathf.Clamp((targetPos - (Vector2)transform.position).magnitude, 0.1f, 10f);
        float duration = Mathf.Lerp(1.5f, 3f, distance / 10f);

        float t = 0;
        int bulletCnt = (int)data.DashBulletCount;
        float shootCycle = 0.5f;

        Tween moveT = controller.transform.DOMove(targetPos, duration).SetEase(Ease.InOutExpo);
        moveT.OnUpdate(()=>
        {
            t += Time.deltaTime;
            if(t > shootCycle)
            {
                t = 0;
                SpawnBullet();
            }   
        });
        
        Tween rotateT = controller.transform.DORotate(new Vector3(0, 360, 0), shootCycle, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops((int)(duration / shootCycle));    

        Sequence sequence = DOTween.Sequence();
        sequence.Append(moveT);
       // sequence.Join(rotateT);
        sequence.OnComplete(() =>
        {
            DashEndEvent();
        });         
    }

    private void DashEndEvent()
    {
        data.SetDashCoolDown();
        con.ChangeState(ETestEnemyState.Idle);
    }

    private void SpawnBullet()
    {
        int cnt = (int)data.DashBulletCount;
        float angle = 360 / cnt;
        Debug.Log("Angle " + angle);
        for (int i = 0; i < cnt; i++)
        {
            EEnemyBulletCurveType type = EEnemyBulletCurveType.Curve180;

            float curAngle = angle * i * Mathf.Deg2Rad;
            con.InstantiateBullet(new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle)), EEnemyBulletSpeedType.Linear, type);
        }
    }

    protected override void ExitState()
    {
        base.ExitState();
    }

    protected override void UpdateState()
    {
        base.UpdateState();
    }
}
