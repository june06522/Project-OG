using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHalfHPState : BossBaseState
{
    private SlateBoss _slate;
    private GameObject[] g_minimis;
    private LineRenderer[] _minimiLaserLineRenderer;
    public SHalfHPState(SlateBoss boss) : base(boss)
    {
        _slate = boss;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        g_minimis = new GameObject[_slate.MinimiCount];
        _minimiLaserLineRenderer = new LineRenderer[_slate.MinimiCount];
        _slate.GetComponent<SpriteRenderer>().sprite = _slate.L_sprite[1];
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
            g_minimis[i].transform.localPosition = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / g_minimis.Length), Mathf.Sin(Mathf.PI * 2 * i / g_minimis.Length)).normalized * _slate.F_minimiAwayDistance;
            g_minimis[i].transform.rotation = Quaternion.identity;
            _minimiLaserLineRenderer[i] = g_minimis[i].GetComponent<LineRenderer>();
            _minimiLaserLineRenderer[i].material = _slate.L_materials[2];
        }
    }

    private IEnumerator RandomPattern(float waitTime)
    {
        if (!_slate.B_halfHP)
            yield break;

        yield return new WaitForSeconds(waitTime);

        int rand = Random.Range(1, 6);

        _slate.B_isRunning = true;

        switch (rand)
        {
            case 1:
                NowCoroutine(Laser(1, 5));
                break;
            case 2:
                NowCoroutine(TornadoShot(15, 5, 0.1f, 3));
                break;
            case 3:
                NowCoroutine(StarAttack(32, 5, 1f, g_minimis.Length));
                break;
            case 4:
                NowCoroutine(RandomMoveAttack(10, 5, 2, 3));
                break;
            case 5:
                NowCoroutine(StopAndGoAttack(20, 3, 1, 3));
                break;
        }
    }

    // 레이저 라인 랜더러로 바꾸기
    private IEnumerator Laser(float warningTime, float fireTime)
    {
        if (!_slate.B_halfHP)
            yield break;

        _slate.B_isStop = true;

        float curTime = 0;
        float deg = 0;
        Vector2 dir = Vector2.zero;

        for (int i = 0; i < g_minimis.Length; i++)
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
            if (i % 2 == 0)
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
                        var x = Mathf.Cos(rad);
                        var y = Mathf.Sin(rad);

                        dir = new Vector2(x, y).normalized;
                        _minimiLaserLineRenderer[i].SetPosition(1, RayWallCheck(g_minimis[i].transform.position, dir));
                        RayPlayerCheck(g_minimis[i].transform.position, dir);
                    }
                    else if(i == 1)
                    {
                        var x = -Mathf.Cos(rad);
                        var y = Mathf.Sin(rad);

                        dir = new Vector2(x, y).normalized;
                        _minimiLaserLineRenderer[i].SetPosition(1, RayWallCheck(g_minimis[i].transform.position, dir));
                        RayPlayerCheck(g_minimis[i].transform.position, dir);
                    }
                    else
                    {
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

        for (int i = 0; i < _minimiLaserLineRenderer.Length; i++)
        {
            _minimiLaserLineRenderer[i].enabled = false;
        }

        _slate.B_isStop = false;
        _slate.B_isRunning = false;
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

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<IHitAble>(out var hitAble))
            {
                Debug.Log("레이저");
                hitAble.Hit(_slate.bossSo.Damage);
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
        if (!_slate.B_halfHP)
            yield break;

        for (int i = 0; i < turnCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                for (int k = 0; k < g_minimis.Length; k++)
                {
                    if (k % 2 == 0)
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

        _slate.B_isRunning = false;
        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
    }

    private void TornadoShotBulletsMake(Vector3 pos, int bulletCount, int nowNum, float speed, int multiply)
    {
        GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _slate.G_bulletCollector.transform);
        bullet.GetComponent<BossBullet>().Attack(_slate.bossSo.Damage);
        bullet.GetComponent<SpriteRenderer>().material = _slate.L_materials[3];
        bullet.transform.position = pos;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dir = new Vector2(multiply * Mathf.Cos(Mathf.PI * 2 * nowNum / bulletCount), multiply * Mathf.Sin(Mathf.PI * 2 * nowNum / bulletCount));
        rigid.velocity = dir.normalized * speed;
    }

    private IEnumerator StarAttack(int bulletCount, float speed, float time, int burstCount)
    {
        if (!_slate.B_halfHP)
            yield break;

        _slate.B_isStop = true;
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
                bullet.GetComponent<BossBullet>().Attack(_slate.bossSo.Damage);
                bullet.GetComponent<SpriteRenderer>().material = _slate.L_materials[3];
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

        _slate.B_isStop = false;
        _slate.B_isRunning = false;
        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
    }

    private IEnumerator RandomMoveAttack(int bulletCount, float speed, float time, int burstCount)
    {
        if (!_slate.B_halfHP)
            yield break;

        GameObject[,,] bullets = new GameObject[g_minimis.Length, burstCount, bulletCount];

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                for (int k = 0; k < g_minimis.Length; k++)
                {
                    bullets[k, i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.SlateBullet, _slate.G_bulletCollector.transform);
                    bullets[k, i, j].GetComponent<BossBullet>().Attack(_slate.bossSo.Damage);
                    bullets[k, i, j].GetComponent<SpriteRenderer>().material = _slate.L_materials[3];
                    bullets[k, i, j].transform.position = g_minimis[k].transform.position;
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
                for (int k = 0; k < g_minimis.Length; k++)
                {
                    Rigidbody2D rigid = bullets[k, i, j].GetComponent<Rigidbody2D>();
                    rigid.velocity = nextDir.normalized * speed;
                    bullets[k, i, j].transform.rotation = Quaternion.identity;
                    bullets[k, i, j].transform.Rotate(new Vector3(0, 0, lookDeg));
                }
            }
        }

        _slate.B_isRunning = false;
        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
    }

    private IEnumerator StopAndGoAttack(int bulletCount, float speed, float time, int burstCount)
    {
        if (!_slate.B_halfHP)
            yield break;
        GameObject[,] bullets = new GameObject[burstCount, bulletCount];

        int returnCounting = 1;

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                bullets[i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _slate.G_bulletCollector.transform);
                bullets[i, j].GetComponent<BossBullet>().Attack(_slate.bossSo.Damage);
                bullets[i, j].GetComponent<SpriteRenderer>().material = _slate.L_materials[3];
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
                    bullets[i - 1, j].GetComponent<SpriteRenderer>().material = _slate.L_materials[3];
                    Rigidbody2D rigid = bullets[i - 1, j].GetComponent<Rigidbody2D>();
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                    rigid.velocity = dir.normalized * speed;
                }

            for (int j = 0; j < bulletCount; j++)
            {
                bullets[i, j].GetComponent<SpriteRenderer>().material = _slate.L_materials[4];
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
                bullets[counting, i].GetComponent<SpriteRenderer>().material = _slate.L_materials[4];
                Rigidbody2D rigid = bullets[counting, i].GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.zero;
            }

            if (counting > 0)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    bullets[counting - 1, i].GetComponent<SpriteRenderer>().material = _slate.L_materials[3];
                    Rigidbody2D rigid = bullets[counting - 1, i].GetComponent<Rigidbody2D>();
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bulletCount), Mathf.Sin(Mathf.PI * 2 * i / bulletCount));
                    rigid.velocity = dir.normalized * speed;
                }
            }
            else if (counting == 0)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    bullets[burstCount - 1, i].GetComponent<SpriteRenderer>().material = _slate.L_materials[3];
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

        yield return new WaitForSeconds(time);

        for (int i = 0; i < bulletCount; i++)
        {
            bullets[burstCount - 1, i].GetComponent<SpriteRenderer>().material = _slate.L_materials[3];
            Rigidbody2D rigid = bullets[burstCount - 1, i].GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bulletCount), Mathf.Sin(Mathf.PI * 2 * i / bulletCount));
            rigid.velocity = dir.normalized * speed;
        }


        _slate.B_isRunning = false;
        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
    }
}
