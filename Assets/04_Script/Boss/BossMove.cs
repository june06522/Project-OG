using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    public Boss boss;

    public bool isArrive;

    private bool _stop = false;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(boss.transform.position, boss.so.WallCheckRadius);
    }

    public void StopMovement(bool b)
    {
        _stop = b;
    }

    public IEnumerator BossMovement(float waitTime, float maxX, float minX, float maxY, float minY, float speed, float wallCheckRadius)
    {
        Vector3 targetpatrolPos = transform.localPosition;
        bool wallChecked = false;
        _stop = false;

        while (!boss.IsDie && !_stop)
        {
            if (RayWallCheckForMove(transform.position, wallCheckRadius) && !wallChecked)
            {
                wallChecked = true;
                yield return new WaitForSeconds(waitTime);
                targetpatrolPos = MakeNewTargetPos(maxX, minX, maxY, minY);
            }

            if (Arrive(transform.localPosition, targetpatrolPos))
            {
                isArrive = true;
                if (wallChecked)
                {
                    wallChecked = false;
                }

                yield return new WaitForSeconds(waitTime);
                isArrive = false;
                targetpatrolPos = MakeNewTargetPos(maxX, minX, maxY, minY);
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

        _stop = false;
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

    private Vector3 MakeNewTargetPos(float maxX, float minX, float maxY, float minY)
    {
        Vector3 newPatrolPos = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));

        return newPatrolPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            MakeNewTargetPos(boss.so.MoveX, -boss.so.MoveX, boss.so.MoveY, -boss.so.MoveY);
        }
    }
}
