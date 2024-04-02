using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHalfHPState : BossBaseState
{
    private SlateBoss _slate;
    private SlatePattern _pattern;
    private GameObject[] g_minimis;
    private LineRenderer[] _minimiLaserLineRenderer;
    private Vector3[] _originPos;

    public SHalfHPState(SlateBoss boss, SlatePattern pattern) : base(boss, pattern)
    {
        _slate = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        _slate.ReturnMinimi(g_minimis);
    }

    public override void OnBossStateOn()
    {
        _slate.isAttacking = false;
        g_minimis = new GameObject[_slate.MinimiCount];
        _minimiLaserLineRenderer = new LineRenderer[_slate.MinimiCount];
        _originPos = new Vector3[_slate.MinimiCount];
        _slate.GetComponent<SpriteRenderer>().sprite = _slate.halfHPSprite;
        CreateMinimi();
        _slate.StartCoroutine(NowMove(0.5f));
        _slate.StartCoroutine(RandomPattern(_slate.so.PatternChangeTime));
        _slate.StartCoroutine(_slate.bossMove.BossMovement(_slate.so.StopTime, _slate.so.MoveX, _slate.so.MoveY, _slate.so.Speed, _slate.so.WallCheckRadius));
    }

    public override void OnBossStateUpdate()
    {

    }

    private void CreateMinimi()
    {
        for (int i = 0; i < g_minimis.Length; i++)
        {
            g_minimis[i] = ObjectPool.Instance.GetObject(ObjectPoolType.SlateMinimi, _slate.transform);
            g_minimis[i].GetComponent<SpriteRenderer>().material = _slate.minimiBasicMat;
            _originPos[i] = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / g_minimis.Length), Mathf.Sin(Mathf.PI * 2 * i / g_minimis.Length)).normalized * _slate.minimiAwayDistance;
            g_minimis[i].transform.localPosition = _originPos[i];
            g_minimis[i].transform.rotation = Quaternion.identity;
            _minimiLaserLineRenderer[i] = g_minimis[i].GetComponent<LineRenderer>();
            _minimiLaserLineRenderer[i].material = _slate.laserMat;

        }
    }

    private IEnumerator NowMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _slate.isStop = false;
    }

    private IEnumerator RandomPattern(float waitTime)
    {
        while(_slate.halfHP)
        {
            if (_slate.isAttacking)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(waitTime);
            Debug.Log("pattern");

            int rand = Random.Range(1, 6);

            _slate.isAttacking = true;

            switch (rand)
            {
                case 1:
                    NowCoroutine(_pattern.Laser(_slate, g_minimis, _minimiLaserLineRenderer, _originPos, 1, 5, 50, 1, 50, _slate.halfHP));
                    break;
                case 2:
                    NowCoroutine(_pattern.TornadoShot(_slate, g_minimis, 15, 5, 0.1f, 3, _slate.halfHP));
                    break;
                case 3:
                    NowCoroutine(_pattern.StarAttack(_slate, 32, 5, 1f, g_minimis.Length, _slate.halfHP));
                    break;
                case 4:
                    NowCoroutine(_pattern.RandomMoveAttack(_slate, g_minimis, 10, 5, 2, 3, _slate.halfHP));
                    break;
                case 5:
                    NowCoroutine(_pattern.StopAndGoAttack(_slate, 20, 3, 1, 3, _slate.halfHP));
                    break;
            }
        }
    }
}
