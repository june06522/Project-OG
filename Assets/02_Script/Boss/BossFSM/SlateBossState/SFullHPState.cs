using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFullHPState : BossBaseState
{
    private SlateBoss _slate;
    private GameObject[] g_minimis = new GameObject[2];
    private LineRenderer[] _minimiLaserLineRenderer = new LineRenderer[2];
    Vector2 dir = Vector2.zero;

    public SFullHPState(SlateBoss boss) : base(boss)
    {
        _slate = boss;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        CreateMinimi();
        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private void CreateMinimi()
    {
        for (int i = 0; i < g_minimis.Length; i++)
        {
            g_minimis[i] = ObjectPool.Instance.GetObject(ObjectPoolType.SlateMinimi, _boss.transform);
            g_minimis[i].transform.position = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / g_minimis.Length), Mathf.Sin(Mathf.PI * 2 * i / g_minimis.Length)).normalized * _slate.F_minimiAwayDistance;
            g_minimis[i].transform.rotation = Quaternion.identity;
            _minimiLaserLineRenderer[i] = g_minimis[i].GetComponent<LineRenderer>();
            _minimiLaserLineRenderer[i].material = _slate.L_materials[2];
        }
    }

    private IEnumerator RandomPattern(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        int rand = Random.Range(1, 6);

        switch(rand)
        {
            case 1:
                NowCoroutine(Laser(1, 10));
                break;
            case 2:
                NowCoroutine(TornadoShot(30, 5, 0.1f, 3));
                break;
            case 3:
                NowCoroutine(StarAttack(32, 5, 1f, g_minimis.Length));
                break;
            case 4:
                NowCoroutine(RandomMoveAttack(20, 5, 2, 3));
                break;
            case 5:
                NowCoroutine(StopAndGoAttack(30, 3, 1, 3));
                break;
        }
    }

    // 레이저 라인 랜더러로 바꾸기
    private IEnumerator Laser(float warningTime, float fireTime)
    {
        float curTime = 0;
        float deg = 0;

        for(int i = 0; i < g_minimis.Length; i++)
        {
            g_minimis[i].GetComponent<SpriteRenderer>().material = _slate.L_materials[1];
        }

        yield return new WaitForSeconds(warningTime);

        for (int i = 0; i < g_minimis.Length; i++)
        {
            g_minimis[i].GetComponent<SpriteRenderer>().material = _slate.L_materials[0];
        }

        for (int i = 0; i < g_minimis.Length; i++)
        {
            _minimiLaserLineRenderer[i].SetPosition(0, g_minimis[i].transform.position);
            _minimiLaserLineRenderer[i].startWidth = 0.1f;
            if(i % 2 == 0)
            {
                ShowLineRenderer(g_minimis[i].transform.position, _minimiLaserLineRenderer[i], Vector2.right, 0.1f);
            }
            else
            {
                ShowLineRenderer(g_minimis[i].transform.position, _minimiLaserLineRenderer[i], Vector2.left, 0.1f);
            }
        }


        while (curTime < fireTime)
        {
            curTime += Time.deltaTime;
            deg += Time.deltaTime * 100;

            if (deg < 360)
            {
                var rad = Mathf.Deg2Rad * (deg + 360);

                for (int i = 0; i < _minimiLaserLineRenderer.Length; i++)
                {
                    if (i == 0)
                    {
                        bool once = false;
                        var x = Mathf.Cos(rad);
                        var y = Mathf.Sin(rad);

                        dir = new Vector2(x, y).normalized;
                        _minimiLaserLineRenderer[i].SetPosition(1, RayWallCheck(g_minimis[i].transform.position, dir));
                        RayPlayerCheck(g_minimis[i].transform.position, dir);
                    }
                    else
                    {
                        bool once = false;
                        var x = -Mathf.Cos(rad);
                        var y = -Mathf.Sin(rad);

                        dir = new Vector2(x, y).normalized;
                        _minimiLaserLineRenderer[i].SetPosition(1, RayWallCheck(g_minimis[i].transform.position, dir));
                        RayPlayerCheck(g_minimis[i].transform.position, dir);
                    }
                }
            }
            else
            {
                deg = 0;
            }

            yield return null;
        }

        for(int i = 0; i < _minimiLaserLineRenderer.Length; i++)
        {
            _minimiLaserLineRenderer[i].enabled = false;
        }

        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
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

    private void RayPlayerCheck(Vector3 pos, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, Mathf.Infinity, LayerMask.GetMask("Player"));

        if(hit.collider != null)
        {
            if (hit.collider.TryGetComponent<IHitAble>(out var hitAble))
            {
                Debug.Log("레이저");
                hitAble.Hit(_slate.bossSo.Damage);
            }
            else
            {
                Debug.Log(hit.collider.name);
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

    private IEnumerator TornadoShot(int bulletCount, float speed, float time, int turnCount)
    {
        for (int i = 0; i < turnCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                for(int k = 0; k < g_minimis.Length; k++)
                {
                    if(k % 2 == 0)
                    {
                        TornadoShotBulletsMake(g_minimis[k].transform.position, bulletCount, j, speed, 1);
                    }
                    else
                    {
                        TornadoShotBulletsMake(g_minimis[k].transform.position, bulletCount, j, speed, -1);
                    }
                }

                yield return new WaitForSeconds(time);
            }
        }

        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
    }

    private void TornadoShotBulletsMake(Vector3 pos, int bulletCount, int nowNum, float speed, int multiply)
    {
        GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _slate.G_bulletCollector.transform);
        bullet.transform.position = pos;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dir = new Vector2(multiply * Mathf.Cos(Mathf.PI * 2 * nowNum / bulletCount), multiply * Mathf.Sin(Mathf.PI * 2 * nowNum / bulletCount));
        rigid.velocity = dir.normalized * speed;
    }

    private IEnumerator StarAttack(int bulletCount, float speed, float time, int burstCount)
    {
        bool plus = true;
        float r = 1;

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                if (r > 1.1f)
                    plus = false;
                else if (r == 1)
                    plus = true;
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _slate.G_bulletCollector.transform);
                bullet.transform.position = _slate.transform.position;
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

        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
    }

    private IEnumerator RandomMoveAttack(int bulletCount, float speed, float time, int burstCount)
    {
        GameObject[, ,] bullets = new GameObject[g_minimis.Length, burstCount, bulletCount];

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                for(int k = 0; k < g_minimis.Length; k++)
                {
                    bullets[k, i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _slate.G_bulletCollector.transform);
                    bullets[k, i, j].transform.position = g_minimis[k].transform.position;
                    bullets[k, i, j].transform.rotation = Quaternion.identity;

                    Rigidbody2D rigid = bullets[k, i, j].GetComponent<Rigidbody2D>();
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                    rigid.velocity = dir.normalized * speed;
                }
            }

            yield return new WaitForSeconds(time);

            int rand = Random.Range(1, 5);
            Vector2 nextDir;
            switch (rand)
            {
                case 1:
                    nextDir = Vector2.up;
                    break;
                case 2:
                    nextDir = Vector2.down;
                    break;
                case 3:
                    nextDir = Vector2.left;
                    break;
                case 4:
                    nextDir = Vector2.right;
                    break;
                default:
                    nextDir = Vector2.left;
                    break;
            }

            for (int j = 0; j < bulletCount; j++)
            {
                for(int k = 0; k < g_minimis.Length; k++)
                {
                    Rigidbody2D rigid = bullets[k, i, j].GetComponent<Rigidbody2D>();
                    rigid.velocity = nextDir.normalized * speed;
                }
            }
        }

        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
    }

    // 갈아버릴지 말지 생각하자 그대로 쓸거면 다른 패턴 총알과 겹쳐서 지랄나는거 고치자
    private IEnumerator StopAndGoAttack(int bulletCount, float speed, float time, int burstCount)
    {
        GameObject[,] bullets = new GameObject[burstCount, bulletCount];

        int returnCounting = 1;

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                bullets[i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _slate.G_bulletCollector.transform);
                bullets[i, j].transform.position = _slate.transform.position;
                bullets[i, j].transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;
            }

            yield return new WaitForSeconds(time);

            if (i > 0)
                for (int j = 0; j < bulletCount; j++)
                {
                    Rigidbody2D rigid = bullets[i - 1, j].GetComponent<Rigidbody2D>();
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                    rigid.velocity = dir.normalized * speed;
                }

            for (int j = 0; j < bulletCount; j++)
            {
                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.zero;
            }
        }

        yield return new WaitForSeconds(time);

        int counting = 0;
        int aCount = 0;

        while (returnCounting < burstCount)
        {
            if (counting > 0)
                for (int i = 0; i < bulletCount; i++)
                {
                    Rigidbody2D rigid = bullets[counting - 1, i].GetComponent<Rigidbody2D>();
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bulletCount), Mathf.Sin(Mathf.PI * 2 * i / bulletCount));
                    rigid.velocity = dir.normalized * speed;
                }
            else if (counting == 0)
                for (int i = 0; i < bulletCount; i++)
                {
                    Rigidbody2D rigid = bullets[burstCount - 1, i].GetComponent<Rigidbody2D>();
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bulletCount), Mathf.Sin(Mathf.PI * 2 * i / bulletCount));
                    rigid.velocity = dir.normalized * speed;
                }

            for (int i = 0; i < bulletCount; i++)
            {
                Rigidbody2D rigid = bullets[counting, i].GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.zero;
            }

            if (counting >= burstCount - 1)
                counting = 0;
            else
                counting++;

            aCount++;

            yield return new WaitForSeconds(time);
        }

        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
    }
}
