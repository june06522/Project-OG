using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHalfHPState : BossBaseState
{
    private SlateBoss _slate;
    private GameObject[] g_minimis;
    private LineRenderer[] _minimiLaserLineRenderer;
    private Vector3[] v_originPos;

    public SHalfHPState(SlateBoss boss) : base(boss)
    {
        _slate = boss;
    }

    public override void OnBossStateExit()
    {
        _slate.ReturnMinimi(g_minimis);
    }

    public override void OnBossStateOn()
    {
        _slate.B_patorl = true;
        _slate.B_isStop = false;
        g_minimis = new GameObject[_slate.MinimiCount];
        _minimiLaserLineRenderer = new LineRenderer[_slate.MinimiCount];
        v_originPos = new Vector3[_slate.MinimiCount];
        _slate.GetComponent<SpriteRenderer>().sprite = _slate.L_sprite[1];
        CreateMinimi();
        _slate.StartCoroutine(NowMove(0.5f));
        _slate.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
        _slate.StartCoroutine(_slate.BossPatorl(_slate.bossSo.StopTime, _slate.bossSo.MoveX, _slate.bossSo.MoveY, _slate.bossSo.Speed));
    }

    public override void OnBossStateUpdate()
    {

    }

    private void CreateMinimi()
    {
        for (int i = 0; i < g_minimis.Length; i++)
        {
            g_minimis[i] = ObjectPool.Instance.GetObject(ObjectPoolType.SlateMinimi, _boss.transform);
            g_minimis[i].GetComponent<SpriteRenderer>().material = _slate.L_materials[5];
            v_originPos[i] = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / g_minimis.Length), Mathf.Sin(Mathf.PI * 2 * i / g_minimis.Length)).normalized * _slate.F_minimiAwayDistance;
            g_minimis[i].transform.localPosition = v_originPos[i];
            g_minimis[i].transform.rotation = Quaternion.identity;
            _minimiLaserLineRenderer[i] = g_minimis[i].GetComponent<LineRenderer>();
            _minimiLaserLineRenderer[i].material = _slate.L_materials[2];

        }
    }

    private IEnumerator NowMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _slate.B_isStop = false;
    }

    private IEnumerator RandomPattern(float waitTime)
    {
        if (!_slate.B_halfHP)
            yield break;

        yield return new WaitForSeconds(waitTime);

        int rand = Random.Range(1, 6);

        switch (rand)
        {
            case 1:
                NowCoroutine(Laser(1, 5, 50, 1, 50));
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

    private IEnumerator Laser(float warningTime, float fireTime, float speed, float goBackTime, float minimiMoveSpeed)
    {
        if (!_slate.B_halfHP)
            yield break;

        _slate.B_isStop = true;

        float curTime = 0;
        float deg = 90;
        Vector2 dir = Vector2.zero;

        for (int i = 0; i < g_minimis.Length; i++)
        {
            g_minimis[i].GetComponent<SpriteRenderer>().material = _slate.L_materials[1];
            g_minimis[i].transform.SetParent(null);
        }

        Vector3[] target = new Vector3[g_minimis.Length];

        int rand = Random.Range(0, 8);
        int sum = 0;

        for(int i = 0; i < g_minimis.Length; i++)
        {
            if(rand + sum + i <= 7)
            {
                target[i] = _slate.G_minimisPositions.transform.GetChild(rand + sum + i).transform.position;
                sum += i + 1;
            }
            else
            {
                target[i] = _slate.G_minimisPositions.transform.GetChild(rand + sum + i - 8).transform.position;
                sum += i + 1;
            }
        }

        while (curTime < warningTime / 2)
        {
            curTime += Time.deltaTime;
            for (int i = 0; i < g_minimis.Length; i++)
            {
                if (i == 0)
                {
                    g_minimis[i].transform.position = Vector3.MoveTowards(g_minimis[i].transform.position, target[i], Time.deltaTime * minimiMoveSpeed);
                }
                else
                {
                    g_minimis[i].transform.position = Vector3.MoveTowards(g_minimis[i].transform.position, target[i], Time.deltaTime * minimiMoveSpeed);
                }
            }

            yield return null;
        }

        for (int i = 0; i < g_minimis.Length; i++)
        {
            SetLineMaterial(_slate.L_materials[1]);
            _minimiLaserLineRenderer[i].SetPosition(0, RayWallCheck(g_minimis[i].transform.position, Vector2.down));
            _minimiLaserLineRenderer[i].startWidth = 0.05f;
            ShowLineRenderer(g_minimis[i].transform.position, _minimiLaserLineRenderer[i], Vector2.up, 0.05f);
        }

        yield return new WaitForSeconds(warningTime);

        curTime = 0;
        SetLineMaterial(_slate.L_materials[2]);
        for (int i = 0; i < g_minimis.Length; i++)
        {
            g_minimis[i].GetComponent<SpriteRenderer>().material = _slate.L_materials[0];
        }

        SoundManager.Instance.SFXPlay("Laser", _slate.audios[2], _boss.G_bulletCollector.transform, 0.3f);

        while (curTime < fireTime)
        {
            curTime += Time.deltaTime;
            deg += Time.deltaTime * speed;

            if (deg < 360)
            {
                var rad = Mathf.Deg2Rad * (deg + 360);

                for (int i = 0; i < g_minimis.Length; i++)
                {
                    if (i == 0)
                    {
                        var x = Mathf.Cos(rad);
                        var y = Mathf.Sin(rad);

                        dir = new Vector2(x, y).normalized;
                        _minimiLaserLineRenderer[i].SetPosition(0, RayWallCheck(g_minimis[i].transform.position, -dir));
                        _minimiLaserLineRenderer[i].SetPosition(1, RayWallCheck(g_minimis[i].transform.position, dir));
                        RayPlayerCheck(g_minimis[i].transform.position, -dir);
                        RayPlayerCheck(g_minimis[i].transform.position, dir);
                    }
                    else
                    {
                        var x = -Mathf.Cos(rad);
                        var y = Mathf.Sin(rad);

                        dir = new Vector2(x, y).normalized;
                        _minimiLaserLineRenderer[i].SetPosition(0, RayWallCheck(g_minimis[i].transform.position, -dir));
                        _minimiLaserLineRenderer[i].SetPosition(1, RayWallCheck(g_minimis[i].transform.position, dir));
                        RayPlayerCheck(g_minimis[i].transform.position, -dir);
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

        for (int i = 0; i < g_minimis.Length; i++)
        {
            _minimiLaserLineRenderer[i].enabled = false;
            g_minimis[i].GetComponent<SpriteRenderer>().material = _slate.L_materials[6];
        }

        yield return new WaitForSeconds(goBackTime);

        for(int i = 0; i < g_minimis.Length; i++)
        {
            g_minimis[i].transform.SetParent(_slate.transform);
        }

        curTime = 0;

        while (curTime < warningTime / 2)
        {
            curTime += Time.deltaTime;
            for (int i = 0; i < g_minimis.Length; i++)
            {
                if (i == 0)
                {
                    g_minimis[i].transform.localPosition = Vector3.MoveTowards(g_minimis[i].transform.localPosition, v_originPos[i], Time.deltaTime * minimiMoveSpeed);
                }
                else
                {
                    g_minimis[i].transform.localPosition = Vector3.MoveTowards(g_minimis[i].transform.localPosition, v_originPos[i], Time.deltaTime * minimiMoveSpeed);
                }
            }

            yield return null;
        }

        for (int i = 0; i < g_minimis.Length; i++)
        {
            g_minimis[i].GetComponent<SpriteRenderer>().material = _slate.L_materials[5];
        }

        _slate.B_isStop = false;
        _slate.B_isRunning = false;
        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
    }

    private void SetLineMaterial(Material mat)
    {
        for (int i = 0; i < g_minimis.Length; i++)
        {
            _minimiLaserLineRenderer[i].material = mat;
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

    private IEnumerator TornadoShot(int bulletCount, float speed, float time, int turnCount)
    {
        if (!_slate.B_halfHP)
            yield break;

        for (int i = 0; i < turnCount; i++)
        {
            SoundManager.Instance.SFXPlay("Fire", _slate.audios[1]);
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
            SoundManager.Instance.SFXPlay("Fire", _slate.audios[1]);
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
        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
    }

    private IEnumerator RandomMoveAttack(int bulletCount, float speed, float time, int burstCount)
    {
        if (!_slate.B_halfHP)
            yield break;

        GameObject[,,] bullets = new GameObject[g_minimis.Length, burstCount, bulletCount];

        for (int i = 0; i < burstCount; i++)
        {
            SoundManager.Instance.SFXPlay("Fire", _slate.audios[1]);
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
            SoundManager.Instance.SFXPlay("Fire", _slate.audios[1]);
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

        _slate.StartCoroutine(RandomPattern(_slate.bossSo.PatternChangeTime));
    }
}
