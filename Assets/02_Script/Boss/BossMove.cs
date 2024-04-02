using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    public Boss boss;

    public IEnumerator BossMovement(float waitTime, float randX, float randY, float speed, float wallCheckRadius)
    {
        Vector3 targetpatrolPos = transform.localPosition;
        bool wallChecked = false;

        while (!boss.IsDie)
        {
            
            if (RayWallCheckForMove(transform.position, wallCheckRadius) && !wallChecked)
            {
                wallChecked = true;
                yield return new WaitForSeconds(waitTime);
                targetpatrolPos = (GameManager.Instance.player.transform.position - transform.position).normalized;
            }

            if (Arrive(transform.localPosition, targetpatrolPos))
            {
                if (wallChecked)
                {
                    wallChecked = false;
                }
                yield return new WaitForSeconds(waitTime);
                targetpatrolPos = MakeNewTargetPos(randX, randY);
            }
            else
            {
                if (boss.isStop)
                {
                    targetpatrolPos = transform.localPosition;
                }
                else
                {
                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetpatrolPos, Time.deltaTime * speed);
                }
            }

            yield return null;
        }
    }

    private bool RayWallCheckForMove(Vector3 originPos, float radius)
    {
        Collider2D hit = Physics2D.OverlapCircle(originPos, radius, LayerMask.GetMask("Wall"));

        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool Arrive(Vector3 myPos, Vector3 targetPos)
    {
        if (Mathf.Abs(Vector3.Distance(myPos, targetPos)) <= 0.5f)
            return true;

        return false;
    }

    private Vector3 MakeNewTargetPos(float randX, float randY)
    {
        Vector3 newPatrolPos = new Vector2(UnityEngine.Random.Range(-randX, randX), UnityEngine.Random.Range(-randY, randY));

        return newPatrolPos;
    }
}
