using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlatePattern : BossPatternBase
{
    public IEnumerator Laser(SlateBoss boss, GameObject[] objs, LineRenderer[] lines, Vector3[] pos, float warningTime, float fireTime, float speed, float goBackTime, float minimiMoveSpeed, bool breaker)
    {
        if (!breaker)
            yield break;

        boss.isStop = true;

        float curTime = 0;
        float deg = 90;
        Vector2 dir = Vector2.zero;

        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].GetComponent<SpriteRenderer>().material = boss.warningMat;
            objs[i].transform.SetParent(null);
        }

        Vector3[] target = new Vector3[objs.Length];

        int rand = Random.Range(0, 8);
        int sum = 0;

        for (int i = 0; i < objs.Length; i++)
        {
            if (rand + sum + i <= 7)
            {
                target[i] = boss.minimisPositions.transform.GetChild(rand + sum + i).transform.position;
                sum += i + 1;
            }
            else
            {
                target[i] = boss.minimisPositions.transform.GetChild(rand + sum + i - 8).transform.position;
                sum += i + 1;
            }
        }

        while (curTime < warningTime / 2)
        {
            curTime += Time.deltaTime;
            for (int i = 0; i < objs.Length; i++)
            {
                if (i == 0)
                {
                    objs[i].transform.position = Vector3.MoveTowards(objs[i].transform.position, target[i], Time.deltaTime * minimiMoveSpeed);
                }
                else
                {
                    objs[i].transform.position = Vector3.MoveTowards(objs[i].transform.position, target[i], Time.deltaTime * minimiMoveSpeed);
                }
            }

            yield return null;
        }

        for (int i = 0; i < objs.Length; i++)
        {
            SetLineMaterial(boss.warningMat, objs, lines);
            lines[i].SetPosition(0, RayWallCheck(objs[i].transform.position, Vector2.down));
            lines[i].startWidth = 0.05f;
            ShowLineRenderer(objs[i].transform.position, lines[i], Vector2.up, 0.05f);
        }

        yield return new WaitForSeconds(warningTime);

        curTime = 0;
        SetLineMaterial(boss.laserMat, objs, lines);
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].GetComponent<SpriteRenderer>().material = boss.StopMat;
        }

        SoundManager.Instance.SFXPlay("Laser", boss.laserClip, boss.bulletCollector.transform, 0.3f);

        while (curTime < fireTime)
        {
            curTime += Time.deltaTime;
            deg += Time.deltaTime * speed;

            if (deg < 360)
            {
                var rad = Mathf.Deg2Rad * (deg + 360);

                for (int i = 0; i < objs.Length; i++)
                {
                    if (i == 0)
                    {
                        var x = Mathf.Cos(rad);
                        var y = Mathf.Sin(rad);

                        dir = new Vector2(x, y).normalized;
                        lines[i].SetPosition(0, RayWallCheck(objs[i].transform.position, -dir));
                        lines[i].SetPosition(1, RayWallCheck(objs[i].transform.position, dir));
                        RayPlayerCheck(objs[i].transform.position, -dir);
                        RayPlayerCheck(objs[i].transform.position, dir);
                    }
                    else
                    {
                        var x = -Mathf.Cos(rad);
                        var y = Mathf.Sin(rad);

                        dir = new Vector2(x, y).normalized;
                        lines[i].SetPosition(0, RayWallCheck(objs[i].transform.position, -dir));
                        lines[i].SetPosition(1, RayWallCheck(objs[i].transform.position, dir));
                        RayPlayerCheck(objs[i].transform.position, -dir);
                        RayPlayerCheck(objs[i].transform.position, dir);
                    }
                }
            }
            else
            {
                deg = 0;
            }

            yield return null;
        }

        for (int i = 0; i < objs.Length; i++)
        {
            lines[i].enabled = false;
            objs[i].GetComponent<SpriteRenderer>().material = boss.hologramMinimiMat;
        }

        yield return new WaitForSeconds(goBackTime);

        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].transform.SetParent(boss.transform);
        }

        curTime = 0;

        while (curTime < warningTime / 2)
        {
            curTime += Time.deltaTime;
            for (int i = 0; i < objs.Length; i++)
            {
                if (i == 0)
                {
                    objs[i].transform.localPosition = Vector3.MoveTowards(objs[i].transform.localPosition, pos[i], Time.deltaTime * minimiMoveSpeed);
                }
                else
                {
                    objs[i].transform.localPosition = Vector3.MoveTowards(objs[i].transform.localPosition, pos[i], Time.deltaTime * minimiMoveSpeed);
                }
            }

            yield return null;
        }

        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].GetComponent<SpriteRenderer>().material = boss.minimiBasicMat;
        }

        boss.isAttacking = false;
        boss.isStop = false;
    }

    private void SetLineMaterial(Material mat, GameObject[] objs, LineRenderer[] lines)
    {
        for (int i = 0; i < objs.Length; i++)
        {
            lines[i].material = mat;
        }
    }

    private void ShowLineRenderer(Vector3 pos, LineRenderer line, Vector2 dir, float scale)
    {
        if (RayWallCheck(pos, dir) != Vector2.zero)
        {
            line.enabled = true;
            line.SetPosition(1, RayWallCheck(pos, dir));
            line.endWidth = scale;
        }
        else
        {
            line.enabled = false;
        }
    }

    // 연속 체크 막기
    private void RayPlayerCheck(Vector3 pos, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, Mathf.Infinity, LayerMask.GetMask("Player"));

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<IHitAble>(out var hitAble))
            {
                hitAble.Hit(1);
            }
        }

    }

    private Vector2 RayWallCheck(Vector3 pos, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, Mathf.Infinity, LayerMask.GetMask("Wall"));

        if (hit.collider != null)
        {
            return hit.point;
        }

        return Vector2.zero;
    }

    public IEnumerator TornadoShot(SlateBoss boss, GameObject[] objs, int bulletCount, float speed, float time, int turnCount, bool breaker)
    {
        if (!breaker)
            yield break;

        for (int i = 0; i < turnCount; i++)
        {
            SoundManager.Instance.SFXPlay("Fire", boss.fireClip);
            for (int j = 0; j < bulletCount; j++)
            {
                for (int k = 0; k < objs.Length; k++)
                {
                    if (k % 2 == 0)
                    {
                        TornadoShotBulletsMake(boss, objs[k].transform.position, bulletCount, j, speed, 1);
                    }
                    else
                    {
                        TornadoShotBulletsMake(boss, objs[k].transform.position, bulletCount, j, speed, -1);
                    }
                }

                yield return new WaitForSeconds(time);
            }
        }

        boss.isAttacking = false;
    }

    private void TornadoShotBulletsMake(SlateBoss boss, Vector3 pos, int bulletCount, int nowNum, float speed, int multiply)
    {
        GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
        bullet.GetComponent<BossBullet>().Attack(boss.so.Damage);
        bullet.GetComponent<SpriteRenderer>().material = boss.bulletMat;
        bullet.transform.position = pos;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dir = new Vector2(multiply * Mathf.Cos(Mathf.PI * 2 * nowNum / bulletCount), multiply * Mathf.Sin(Mathf.PI * 2 * nowNum / bulletCount));
        rigid.velocity = dir.normalized * speed;
    }

    public IEnumerator StarAttack(SlateBoss boss, int bulletCount, float speed, float time, int burstCount, bool breaker)
    {
        if (!breaker)
            yield break;

        boss.isStop = true;
        bool plus = true;
        float r = 1;

        for (int i = 0; i < burstCount; i++)
        {
            SoundManager.Instance.SFXPlay("Fire", boss.fireClip);
            for (int j = 0; j < bulletCount; j++)
            {
                if (r > 1.1f)
                    plus = false;
                else if (r == 1)
                    plus = true;
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
                bullet.GetComponent<BossBullet>().Attack(boss.so.Damage);
                bullet.GetComponent<SpriteRenderer>().material = boss.bulletMat;
                bullet.transform.position = boss.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount)) * r;
                rigid.velocity = dir * speed;

                if (plus)
                    r += 0.1f;
                else
                    r -= 0.1f;
            }

            yield return new WaitForSeconds(time);
        }

        boss.isAttacking = false;
        boss.isStop = false;
    }

    public IEnumerator RandomMoveAttack(SlateBoss boss, GameObject[] objs, int bulletCount, float speed, float time, int burstCount, bool breaker)
    {
        if (!breaker)
            yield break;

        GameObject[,,] bullets = new GameObject[objs.Length, burstCount, bulletCount];

        for (int i = 0; i < burstCount; i++)
        {
            SoundManager.Instance.SFXPlay("Fire", boss.fireClip);
            for (int j = 0; j < bulletCount; j++)
            {
                for (int k = 0; k < objs.Length; k++)
                {
                    bullets[k, i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.SlateBullet, boss.bulletCollector.transform);
                    bullets[k, i, j].GetComponent<BossBullet>().Attack(boss.so.Damage);
                    bullets[k, i, j].GetComponent<SpriteRenderer>().material = boss.bulletMat;
                    bullets[k, i, j].transform.position = objs[k].transform.position;
                    bullets[k, i, j].transform.rotation = Quaternion.identity;

                    Rigidbody2D rigid = bullets[k, i, j].GetComponent<Rigidbody2D>();
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                    rigid.velocity = dir.normalized * speed;

                    Vector3 rotation = Vector3.forward * 360 * j / bulletCount - Vector3.forward * 90;
                    bullets[k, i, j].transform.Rotate(rotation);
                }
            }

            yield return new WaitForSeconds(time);

            int rand = Random.Range(1, 5);
            Vector2 nextDir;
            float lookDeg = 0;
            switch (rand)
            {
                case 1:
                    nextDir = Vector2.up;
                    lookDeg = 0;
                    break;
                case 2:
                    nextDir = Vector2.down;
                    lookDeg = 180;
                    break;
                case 3:
                    nextDir = Vector2.left;
                    lookDeg = 90;
                    break;
                case 4:
                    nextDir = Vector2.right;
                    lookDeg = -90;
                    break;
                default:
                    nextDir = Vector2.left;
                    lookDeg = 90;
                    break;
            }

            for (int j = 0; j < bulletCount; j++)
            {
                for (int k = 0; k < objs.Length; k++)
                {
                    Rigidbody2D rigid = bullets[k, i, j].GetComponent<Rigidbody2D>();
                    rigid.velocity = nextDir.normalized * speed;
                    bullets[k, i, j].transform.rotation = Quaternion.identity;
                    bullets[k, i, j].transform.Rotate(new Vector3(0, 0, lookDeg));
                }
            }
        }

        boss.isAttacking = false;
    }

    public IEnumerator StopAndGoAttack(SlateBoss boss, int bulletCount, float speed, float time, int burstCount, bool breaker)
    {
        if (!breaker)
            yield break;

        GameObject[,] bullets = new GameObject[burstCount, bulletCount];

        int returnCounting = 1;

        for (int i = 0; i < burstCount; i++)
        {
            SoundManager.Instance.SFXPlay("Fire", boss.fireClip);
            for (int j = 0; j < bulletCount; j++)
            {
                bullets[i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
                bullets[i, j].GetComponent<BossBullet>().Attack(boss.so.Damage);
                bullets[i, j].GetComponent<SpriteRenderer>().material = boss.bulletMat;
                bullets[i, j].transform.position = boss.transform.position;
                bullets[i, j].transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;
            }

            yield return new WaitForSeconds(time);

            if (i > 0)
                for (int j = 0; j < bulletCount; j++)
                {
                    bullets[i - 1, j].GetComponent<SpriteRenderer>().material = boss.bulletMat;
                    Rigidbody2D rigid = bullets[i - 1, j].GetComponent<Rigidbody2D>();
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                    rigid.velocity = dir.normalized * speed;
                }

            for (int j = 0; j < bulletCount; j++)
            {
                bullets[i, j].GetComponent<SpriteRenderer>().material = boss.StopMat;
                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.zero;
            }
        }

        yield return new WaitForSeconds(time);

        int counting = 0;

        while (returnCounting <= burstCount)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                bullets[counting, i].GetComponent<SpriteRenderer>().material = boss.StopMat;
                Rigidbody2D rigid = bullets[counting, i].GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.zero;
            }

            if (counting > 0)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    bullets[counting - 1, i].GetComponent<SpriteRenderer>().material = boss.bulletMat;
                    Rigidbody2D rigid = bullets[counting - 1, i].GetComponent<Rigidbody2D>();
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bulletCount), Mathf.Sin(Mathf.PI * 2 * i / bulletCount));
                    rigid.velocity = dir.normalized * speed;
                }
            }
            else if (counting == 0)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    bullets[burstCount - 1, i].GetComponent<SpriteRenderer>().material = boss.bulletMat;
                    Rigidbody2D rigid = bullets[burstCount - 1, i].GetComponent<Rigidbody2D>();
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bulletCount), Mathf.Sin(Mathf.PI * 2 * i / bulletCount));
                    rigid.velocity = dir.normalized * speed;
                }
            }

            if (counting >= burstCount - 1)
                counting = 0;
            else
                counting++;

            returnCounting++;

            yield return new WaitForSeconds(time);
        }

        for (int i = 0; i < bulletCount; i++)
        {
            bullets[burstCount - 1, i].GetComponent<SpriteRenderer>().material = boss.bulletMat;
            Rigidbody2D rigid = bullets[burstCount - 1, i].GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bulletCount), Mathf.Sin(Mathf.PI * 2 * i / bulletCount));
            rigid.velocity = dir.normalized * speed;
        }

        boss.isAttacking = false;
    }
}
