using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPattern : BossPatternBase
{
    public IEnumerator FlowerShapeShot(FlowerBoss boss, int dirCount, int bulletCount, int burstCount, float angle, float speed, float time, bool inPattern)
    {
        for(int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < dirCount; j++)
            {
                Vector2 standard;
                if (i % 2 == 0)
                {
                    standard = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / dirCount), Mathf.Sin(Mathf.PI * 2 * j / dirCount)).normalized;
                }
                else
                {
                    standard = new Vector2(Mathf.Sin(Mathf.PI * 2 * j / dirCount), Mathf.Cos(Mathf.PI * 2 * j / dirCount)).normalized;
                }
                
                MakeLeafShape(boss, standard, bulletCount, angle, speed);
            }

            yield return new WaitForSeconds(time);
        }
        yield return null;
    }

    private void MakeLeafShape(FlowerBoss boss, Vector2 vec, int bulletCount, float angle, float speed)
    {
        int bc = 0;
        if(bulletCount % 2 == 0)
        {
            bc = bulletCount / 2;
        }
        else
        {
            bc = bulletCount / 2 + 1;
        }

        for(int i = -(bulletCount / 2); i < bc; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
            bullet.transform.position = boss.transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = Quaternion.Euler(0, 0, i * angle) * vec;
            rigid.velocity = dir.normalized * speed;
        }
        
    }

    public IEnumerator ScatterShot(FlowerBoss boss, int dirCount, int burstCount, float speed, float angle, float time)
    {
        for (int j = 0; j < dirCount; j++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / dirCount), Mathf.Sin(Mathf.PI * 2 * j / dirCount));
            rigid.velocity = dir.normalized * speed;

            StartCoroutine(Scatter(boss, bullet.transform, speed / 2, dir, burstCount, time, angle));
        }

        yield return null;
    }

    private IEnumerator Scatter(FlowerBoss boss, Transform trans, float speed, Vector2 vec, int burstCount,  float time, float angle)
    {
        GameObject[,] bullets = new GameObject[burstCount, 2];

        Rigidbody2D[] rigids = new Rigidbody2D[2];

        yield return new WaitForSeconds(time);

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                bullets[i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
                bullets[i, j].transform.position = trans.position;
                bullets[i, j].transform.rotation = Quaternion.identity;
                rigids[j] = bullets[i, j].GetComponent<Rigidbody2D>();
            }

            rigids[0].velocity = Quaternion.Euler(0, 0, angle) * vec.normalized * speed;
            rigids[1].velocity = Quaternion.Euler(0, 0, -angle) * vec.normalized * speed;

            yield return new WaitForSeconds(time);
        }
    }

    public IEnumerator RandomOminidirShot(FlowerBoss boss, int burstCount, int bulletCount, float speed, float time)
    {
        for(int i = 0; i < burstCount; i++)
        {
            for(int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
                bullet.transform.position = boss.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * Random.Range(0, 360) / 360), Mathf.Sin(Mathf.PI * 2 * Random.Range(0, 360) / 360));
                rigid.velocity = dir.normalized * speed;
            }

            yield return new WaitForSeconds(time);
        }
    }

    public IEnumerator WarmShot(FlowerBoss boss, int dirCount, int bulletCount, float speed, float time)
    {
        for(int i = 0; i < dirCount; i++)
        {
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / dirCount), Mathf.Sin(Mathf.PI * 2 * i / dirCount));
            StartCoroutine(MakeWarm(boss, dir, bulletCount, speed, time));
        }

        yield return null;
    }

    private IEnumerator MakeWarm(FlowerBoss boss, Vector2 dir, int bulletCount, float speed, float time)
    {
        for (int j = 0; j < bulletCount; j++)
        {
            yield return new WaitForSeconds(time);
            GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType2, boss.bulletCollector.transform);
            bullet.transform.position = boss.transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.velocity = dir.normalized * speed;

        }
    }

    public IEnumerator FullBloomPattern(FlowerBoss boss, int laserCount, float time, float turnSpeed, float turnTime)
    {
        LineRenderer[] lines = new LineRenderer[laserCount];
        GameObject[] lasers = new GameObject[laserCount];
        float curTime = 0;
        float deg = 0;
        
        for(int i = 0; i < laserCount; i++)
        {
            lasers[i] = ObjectPool.Instance.GetObject(ObjectPoolType.Laser, boss.FlowerOnlyCollector.transform);
            lines[i] = lasers[i].GetComponent<LineRenderer>(); 
        }

        lines[0].SetPosition(0, RayWallCheck(boss.transform.position, Vector2.down));
        lines[0].startWidth = 0.1f;
        ShowLineRenderer(boss.transform.position, lines[0], Vector2.up, 0.1f);

        lines[1].SetPosition(0, RayWallCheck(boss.transform.position, Vector2.left));
        lines[1].startWidth = 0.1f;
        ShowLineRenderer(boss.transform.position, lines[1], Vector2.right, 0.1f);

        yield return new WaitForSeconds(1);

        while(curTime < turnTime)
        {
            deg += Time.deltaTime * turnSpeed;
            curTime += Time.deltaTime;

            if(deg < 360)
            {
                for(int i = 0; i < laserCount; i++)
                {
                    var rad = Mathf.Deg2Rad * (deg + i * 360 / (laserCount * 2));

                    var x = Mathf.Cos(rad);
                    var y = Mathf.Sin(rad);

                    Vector2 dir = new Vector2(x, y);

                    lines[i].SetPosition(0, RayWallCheck(boss.transform.position, -dir));
                    lines[i].SetPosition(1, RayWallCheck(boss.transform.position, dir));
                }
            }
            else
            {
                StartCoroutine(FlowerShapeShot(boss, 6, 3, 2, 10, 5, 1, true));
                deg = 0;
            }

            yield return null;
        }
        
        for(int i = 0; i < laserCount; i++)
        {
            lines[i].enabled = false;
        }

        yield return null;
    }
}
