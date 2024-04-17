using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowerPattern : BossPatternBase
{
    public IEnumerator FlowerShapeShot(FlowerBoss boss, int dirCount, int bulletCount, int burstCount, float angle, float speed, float time, bool inPattern, float waitTime)
    {
        boss.isAttacking = true;
        GameObject[] objs = { boss.smallestBody, boss.mediumSizeBody, boss.bigestBody };
        int arrCount = 0;

        for (int i = 0; i < burstCount; i++)
        {
            boss.StartCoroutine(boss.RotateYObject(objs[arrCount], time / 2, 360));
            boss.StartCoroutine(boss.Blinking(objs[arrCount], time, 1, 1, Color.white));
            objs[arrCount].transform.rotation = Quaternion.identity;
            arrCount++;
            yield return new WaitForSeconds(time / 2);
            for (int j = 0; j < dirCount; j++)
            {
                Vector2 standard;
                if (i % 2 == 0)
                {
                    standard = new Vector2(-Mathf.Sin(Mathf.PI * 2 * j / dirCount), -Mathf.Cos(Mathf.PI * 2 * j / dirCount)).normalized;
                }
                else
                {
                    standard = new Vector2(Mathf.Sin(Mathf.PI * 2 * j / dirCount), Mathf.Cos(Mathf.PI * 2 * j / dirCount)).normalized;
                }
                
                MakeLeafShape(boss, standard, bulletCount, angle, speed);
            }
            if(arrCount == objs.Length)
            {
                arrCount = 0;
            }
            yield return new WaitForSeconds(time / 2);
        }

        yield return new WaitForSeconds(waitTime);
        boss.isAttacking = false;
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
            bullet.GetComponent<BossBullet>().Attack(boss.so.Damage);
            bullet.transform.position = boss.transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = Quaternion.Euler(0, 0, i * angle) * vec;
            rigid.velocity = dir.normalized * speed;
        }
    }

    public IEnumerator ScatterShot(FlowerBoss boss, int dirCount, float speed, float waitTime)
    {
        boss.isAttacking = true;

        float animTime = 0.5f;

        boss.bigestBody.transform.DORotate(new Vector3(0, 0, -180), animTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                boss.bigestBody.transform.DORotate(new Vector3(0, 0, 360), animTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    boss.bigestBody.transform.rotation = Quaternion.identity;
                });
            });

        boss.mediumSizeBody.transform.DORotate(new Vector3(0, 0, 180), animTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                boss.mediumSizeBody.transform.DORotate(new Vector3(0, 0, -360), animTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    boss.mediumSizeBody.transform.rotation = Quaternion.identity;
                });
            });

        yield return new WaitForSeconds(animTime / 2);

        for (int j = 0; j < dirCount; j++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.ScatterBullet, boss.bulletCollector.transform);
            bullet.GetComponent<BossBullet>().Attack(boss.so.Damage);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Sin(Mathf.PI * 2 * j / dirCount), Mathf.Cos(Mathf.PI * 2 * j / dirCount));
            rigid.velocity = dir.normalized * speed;
        }

        yield return new WaitForSeconds(waitTime);
        boss.isAttacking = false;
    }

    public IEnumerator RandomOminidirShot(FlowerBoss boss, int burstCount, int bulletCount, float speed, float time, float waitTime)
    {
        boss.isAttacking = true;
        GameObject[] objs = { boss.smallestBody, boss.mediumSizeBody, boss.bigestBody };
        int arrCount = 0;

        for (int i = 0; i < burstCount; i++)
        {
            objs[arrCount].transform.DORotate(new Vector3(0, 0, 180), time / 4)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    boss.StartCoroutine(boss.Blinking(objs[arrCount], time / 2, 1, 1, Color.white));
                });
            
            yield return new WaitForSeconds(time / 2);

            for (int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
                bullet.GetComponent<BossBullet>().Attack(boss.so.Damage);
                bullet.transform.position = boss.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * Random.Range(0, 360) / 360), Mathf.Sin(Mathf.PI * 2 * Random.Range(0, 360) / 360));
                rigid.velocity = dir.normalized * speed;
            }

            objs[arrCount].transform.DORotate(new Vector3(0, 0, 0), time / 4)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => 
                {
                    objs[arrCount].transform.rotation = Quaternion.identity;
                });
            
            arrCount++;

            if (arrCount == objs.Length)
            {
                arrCount = 0;
            }

            yield return new WaitForSeconds(time / 2);
        }

        yield return new WaitForSeconds(waitTime);
        boss.isAttacking = false;
    }

    public IEnumerator WarmShot(FlowerBoss boss, int dirCount, int bulletCount, float speed, float time, float waitTime)
    {
        boss.isAttacking = true;

        float animTime = 0.5f;

        Vector3 bigestOriginSize = boss.bigestBody.transform.localScale;
        Vector3 mediumSizeOriginSize = boss.mediumSizeBody.transform.localScale;
        Vector3 smallestOriginSize = boss.smallestBody.transform.localScale;

        boss.bigestBody.transform.DOScale(bigestOriginSize * 1.5f, animTime / 3)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                boss.mediumSizeBody.transform.DOScale(mediumSizeOriginSize * 1.5f, animTime / 3)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    boss.smallestBody.transform.DOScale(smallestOriginSize * 1.5f, animTime / 3)
                    .SetEase(Ease.InOutSine);
                });
            });

        yield return new WaitForSeconds(animTime);

        for (int i = 0; i < dirCount; i++)
        {
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / dirCount), Mathf.Sin(Mathf.PI * 2 * i / dirCount));
            StartCoroutine(MakeWarm(boss, dir, bulletCount, speed, time));
        }

        boss.bigestBody.transform.DOScale(bigestOriginSize, animTime / 3)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                boss.mediumSizeBody.transform.DOScale(mediumSizeOriginSize, animTime / 3)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    boss.smallestBody.transform.DOScale(smallestOriginSize, animTime / 3)
                    .SetEase(Ease.InOutSine);
                });
            });

        yield return new WaitForSeconds(waitTime);
        boss.isAttacking = false;
    }

    private IEnumerator MakeWarm(FlowerBoss boss, Vector2 dir, int bulletCount, float speed, float time)
    {
        for (int j = 0; j < bulletCount; j++)
        {
            yield return new WaitForSeconds(time);
            GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType2, boss.bulletCollector.transform);
            bullet.GetComponent<BossBullet>().Attack(boss.so.Damage);
            bullet.transform.position = boss.transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.velocity = dir.normalized * speed;
        }
    }

    public IEnumerator FullBloomPattern(FlowerBoss boss, int laserCount, float turnSpeed)
    {
        // 야 이거 애니메이션 해줘
        boss.isAttacking = true;
        LineRenderer[] lines = new LineRenderer[laserCount];
        GameObject[] lasers = new GameObject[laserCount];
        float curTime = 0;
        float deg = 0;
        
        for(int i = 0; i < laserCount; i++)
        {
            lasers[i] = ObjectPool.Instance.GetObject(ObjectPoolType.Laser, boss.flowerOnlyCollector.transform);
            lines[i] = lasers[i].GetComponent<LineRenderer>(); 
        }

        lines[0].SetPosition(0, RayWallCheck(boss.transform.position, Vector2.down));
        lines[0].startWidth = 0.1f;
        ShowLineRenderer(boss.transform.position, lines[0], Vector2.up, 0.1f);

        lines[1].SetPosition(0, RayWallCheck(boss.transform.position, Vector2.left));
        lines[1].startWidth = 0.1f;
        ShowLineRenderer(boss.transform.position, lines[1], Vector2.right, 0.1f);

        yield return new WaitForSeconds(1);

        while(!boss.IsDie)
        {
            deg += Time.deltaTime * turnSpeed;
            curTime += Time.deltaTime;

            boss.bigestBody.transform.rotation = Quaternion.Euler(0, 0, deg);
            boss.mediumSizeBody.transform.rotation = Quaternion.Euler(0, 0, -deg);

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
                    RayPlayerCheck(boss.transform.position, -dir);
                    RayPlayerCheck(boss.transform.position, dir);
                }
            }
            else
            {
                StartCoroutine(FlowerShapeShot(boss, 5, 3, 3, 10, 5, 1, true, 1));
                deg = 0;
            }

            yield return null;
        }
        
        for(int i = 0; i < laserCount; i++)
        {
            lines[i].enabled = false;
        }
        StopAllCoroutines();

        boss.ReturnAll();
        boss.ReturnFlowerCollector();

        boss.isAttacking = false;
        yield return null;
    }
}
