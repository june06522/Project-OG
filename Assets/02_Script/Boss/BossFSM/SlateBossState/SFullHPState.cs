using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFullHPState : BossBaseState
{
    private SlateBoss _slate;
    private GameObject[] g_minimis = new GameObject[4];
    private LineRenderer _line;
    private LineRenderer[] _minimiLaserLineRenderer = new LineRenderer[4];
    Vector2 dir = Vector2.zero;

    public SFullHPState(SlateBoss boss) : base(boss)
    {
        _slate = boss;
        _line = _slate.GetComponent<LineRenderer>();
        _line.material = _slate.L_materials[0];
        _line.material.color = new Color(1, 0, 0, 0.1f);
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
        for (int i = 0; i < 8; i++)
        {
            if(i % 2 != 0)
            {
                g_minimis[i / 2] = ObjectPool.Instance.GetObject(ObjectPoolType.SlateMinimi, _boss.transform);
                g_minimis[i / 2].transform.position = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / 8), Mathf.Sin(Mathf.PI * 2 * i / 8)).normalized * _slate.F_minimiAwayDistance;
                g_minimis[i / 2].gameObject.name = (i / 2).ToString();
                g_minimis[i / 2].transform.rotation = Quaternion.identity;
                _minimiLaserLineRenderer[i / 2] = g_minimis[i / 2].GetComponent<LineRenderer>();
                _minimiLaserLineRenderer[i / 2].material = _slate.L_materials[1];
            }
        }
    }

    private IEnumerator RandomPattern(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        int rand = 1;// Random.Range(1, 5);

        switch(rand)
        {
            case 1:
                NowCoroutine(Laser(1, 100));
                break;
        }
    }

    // 레이저 라인 랜더러로 바꾸기
    private IEnumerator Laser(float warningTime, float fireTime)
    {
        float curTime = 0;
        float deg = 0;
        float subDeg = 0;
        bool isLaser = false;
        _line.SetPosition(0, _slate.transform.position);
        _line.startWidth = 0.1f;

        ShowLineRenderer(_slate.transform.position, _line, Vector2.right, 0.1f);

        while (curTime < fireTime)
        {
            curTime += Time.deltaTime;
            deg += Time.deltaTime * 100;

            if(curTime >= warningTime && !isLaser)
            {
                _line.material = _slate.L_materials[1];
                isLaser = true;
                for(int i = 0; i < g_minimis.Length; i++)
                {
                    _minimiLaserLineRenderer[i].SetPosition(0, g_minimis[i].transform.position);
                    _minimiLaserLineRenderer[i].startWidth = 0.1f;
                    ShowLineRenderer(g_minimis[i].transform.position, _minimiLaserLineRenderer[i], dir, 0.1f);
                }
                curTime = 0;
            }

            if (isLaser)
            {
                // 피해 주는 코드
                for(int i = 0; i < _minimiLaserLineRenderer.Length; i++)
                {
                    RayPlayerCheck(g_minimis[i].transform.position, dir);
                }
                RayPlayerCheck(_slate.transform.position, dir);
            }

            if (deg < 360)
            {
                var rad = Mathf.Deg2Rad * (deg + 360);
                var x = Mathf.Cos(rad);
                var y = Mathf.Sin(rad);

                dir = new Vector2(x, y).normalized;
                if(isLaser)
                {
                    subDeg = deg / 2;
                    if(subDeg < 360)
                    {
                        for (int i = 0; i < _minimiLaserLineRenderer.Length; i++)
                        {
                            // 이거는 조건문 길이 줄이는 방법 좀 생각하자
                            var subRad = Mathf.Deg2Rad * (deg + 360);
                            if(i == 0)
                            {
                                var subX = Mathf.Cos(subRad);
                                var subY = Mathf.Sin(subRad);

                                Vector2 subDir = new Vector2(subX, subY).normalized;
                                _minimiLaserLineRenderer[i].SetPosition(1, RayWallCheck(g_minimis[i].transform.position, subDir));
                            }
                            else if (i == 1)
                            {
                                var subX = -Mathf.Cos(subRad);
                                var subY = Mathf.Sin(subRad);

                                Vector2 subDir = new Vector2(subX, subY).normalized;
                                _minimiLaserLineRenderer[i].SetPosition(1, RayWallCheck(g_minimis[i].transform.position, subDir));
                            }
                            else if(i == 2)
                            {
                                var subX = -Mathf.Cos(subRad);
                                var subY = -Mathf.Sin(subRad);

                                Vector2 subDir = new Vector2(subX, subY).normalized;
                                _minimiLaserLineRenderer[i].SetPosition(1, RayWallCheck(g_minimis[i].transform.position, subDir));
                            }
                            else
                            {
                                var subX = Mathf.Cos(subRad);
                                var subY = -Mathf.Sin(subRad);

                                Vector2 subDir = new Vector2(subX, subY).normalized;
                                _minimiLaserLineRenderer[i].SetPosition(1, RayWallCheck(g_minimis[i].transform.position, subDir));
                            }
                        }
                    }
                    else
                    {
                        subDeg = 0;
                    }
                }
                _line.SetPosition(1, RayWallCheck(_slate.transform.position, dir));
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
        _line.enabled = false;
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
}
