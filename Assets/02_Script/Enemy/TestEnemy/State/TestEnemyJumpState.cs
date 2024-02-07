using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using System.Linq;

public class TestEnemyJumpState : TestEnemyRootState
{
    TestEnemyDataSO data;
    Transform targetTrm;

    Vector2Int jumpGridRange = new(3, 3); // 홀수 범위여야 함

    //Debug
    TestEnemyController con; 

    public TestEnemyJumpState(TestEnemyController controller) : base(controller)
    {
        data = controller.EnemyData;
        targetTrm = GameObject.Find("Player").transform;
        con = controller;
    }

    private void JumpStart()
    {
        Jump();       
    }

    private void Jump()
    {
        Vector2[] ranges = GetRange();
        Vector2  landPos = GetRandomPos(ranges);

        con.InstantiateDebugGrid(landPos);

        con.transform.DOJump(landPos, data.JumpPower, 1, data.JumpDuration)
             .SetEase(Ease.InSine)
             .OnComplete(() =>
             {
                 JumpEndEvent();
             });
    }

    private void JumpEndEvent()
    {
        StartCoroutine(EndCor());
    }

    private IEnumerator EndCor()
    {
        Fire();
        yield return new WaitForSeconds(0.5f);
        Debug.Log("gang");
        data.SetJumpCoolDown();
        con.ChangeState(ETestEnemyState.Idle);
    }

    private void Fire()
    {
        int cnt = (int)data.LandBulletCount;
        float angle = 360 / cnt;
        Debug.Log("Angle " + angle);
        for (int i = 0; i < cnt; i++)
        {
            float curAngle = angle * i * Mathf.Deg2Rad;
            con.InstantiateBullet(new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle)), EEnemyBulletSpeedType.Linear);
        }
    }

    private Vector2 GetRandomPos(Vector2[] ranges)
    {
        if(ranges.Length == 0)
            return con.transform.position;
        return ranges[UnityEngine.Random.Range(0, ranges.Length)];
    }

    private Vector2[] GetRange()
    {
        Vector2[,] lerpGrids = new Vector2[jumpGridRange.x, jumpGridRange.y];

        float lerpGridX = -(jumpGridRange.x / 2);
        float lerpGridY = -(jumpGridRange.y / 2);


        for (int i = 0; i < jumpGridRange.x; i++)
        {
            for (int j = 0; j < jumpGridRange.y; j++)
            {
                lerpGrids[i, j] = new Vector2(lerpGridX + i, lerpGridY + j);
                //Debug.Log($"Lerp Value : X,Y : {lerpGrids[i,j]}");
            }
        }// {-2,-2 } , {-1, -1}


        Vector2[] grids = new Vector2[jumpGridRange.x * jumpGridRange.y];
        Vector2 targetPos = targetTrm.position;
        Debug.Log(targetPos);

        for (int i = 0; i < jumpGridRange.x; i++)
        {
            for (int j = 0; j < jumpGridRange.y; j++)
            {
                Vector2 pos = targetPos + lerpGrids[i, j];
                if (CheckGridInRoom(pos))
                {
                    grids[i * jumpGridRange.y + j] = pos;
                }
            }
        }

        var results = (from grid in grids
                            where grid != default(Vector2)
                            select grid).ToArray();

        //debug
        for (int i = 0; i < results.Length; i++)
        {
            con.InstantiateDebugGrid(results[i]);
        }
        return results;
    }
    
    private bool CheckGridInRoom(Vector2 pos)
    {
        int layer = 1 << 6; //Obstacle  
        return Physics2D.OverlapCircle(pos, 0.2f, layer) == false;
    }

    protected override void EnterState()
    {
        JumpStart();
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
