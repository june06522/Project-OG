using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        _slate.SetBody(_slate.bigestBody, Vector3.one, Vector3.zero, _slate.bossColor, 0.5f);
        _slate.SetBody(_slate.mediumSizeBody, Vector3.one, Vector3.zero, _slate.bossColor, 0.5f);
        _slate.SetBody(_slate.smallestBody, Vector3.one, Vector3.zero, _slate.bossColor, 0.5f);
    }

    public override void OnBossStateOn()
    {
        _slate.SetBody(_slate.bigestBody, Vector3.one, Vector3.zero, _slate.bossColor, 0.5f);
        _slate.SetBody(_slate.mediumSizeBody, Vector3.one, Vector3.zero, _slate.bossColor, 0.5f);
        _slate.SetBody(_slate.smallestBody, Vector3.one, Vector3.zero, _slate.bossColor, 0.5f);

        _slate.gameObject.tag = "Untagged";
        _slate.gameObject.layer = LayerMask.NameToLayer("Default");

        _slate.isAttacking = false;

        g_minimis = new GameObject[_slate.MinimiCount];
        _minimiLaserLineRenderer = new LineRenderer[_slate.MinimiCount];
        _originPos = new Vector3[_slate.MinimiCount];

        _slate.StartCoroutine(HalfAnimation(1.2f));
        _slate.StartCoroutine(ChangeWall(1f));
    }

    public override void OnBossStateUpdate()
    {

    }

    private IEnumerator ChangeWall(float disappearTime)
    {
        Image image = _slate.blinkPanel;
        float curTime = 0;
        float a = 1;
        float speed = a / disappearTime;

        image.gameObject.SetActive(true);
        _slate.smallWall.SetActive(false);

        while(curTime < disappearTime)
        {
            curTime += Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, a -= Time.deltaTime * speed);

            yield return null;
        }

        image.gameObject.SetActive(false);
    }

    private IEnumerator HalfAnimation(float animTime)
    {
        CameraManager.Instance.CameraShake(10, animTime);
        yield return new WaitForSeconds(animTime);

        _slate.StartCoroutine(CreateMinimi());
        _slate.StartCoroutine(NowMove(0.5f));
        _slate.StartCoroutine(RandomPattern(_slate.so.PatternChangeTime));
        _slate.StartCoroutine(_slate.bossMove.BossMovement(_slate.so.StopTime, _slate.so.MoveX, -_slate.so.MoveX, _slate.so.MoveY, -_slate.so.MoveY, _slate.so.Speed, _slate.so.WallCheckRadius));

        _slate.gameObject.tag = "HitAble";
    }

    private IEnumerator CreateMinimi()
    {
        CameraManager.Instance.Shockwave(_slate.transform.position, 0.2f, 0.4f, 0.3f);

        yield return new WaitForSeconds(0.3f);

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

        CameraManager.Instance.CameraShake(10, 0.2f);
        _slate.gameObject.layer = LayerMask.NameToLayer("Boss");
    }

    private IEnumerator NowMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _slate.isStop = false;
    }

    private IEnumerator RandomPattern(float waitTime)
    {
        int beforeRand = 0;

        while (_slate.halfHP)
        {
            if (_slate.isAttacking)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(waitTime);

            int rand = Random.Range(1, 5);
            if (beforeRand == rand)
            {
                if (rand == 1)
                {
                    rand = rand + 1;
                }
                else if (rand == 4)
                {
                    rand = Random.Range(1, 4);
                }
                else
                {
                    rand = rand - 1;
                }
            }
            beforeRand = rand;

            _slate.isAttacking = true;

            switch (rand)
            {
                case 1:
                    NowCoroutine(_pattern.Laser(_slate, g_minimis, _minimiLaserLineRenderer, _originPos, 1, 5, 50, 1, 50, _slate.halfHP));
                    break;
                case 2:
                    NowCoroutine(_pattern.TornadoShot(_slate, g_minimis, 10, 5, 0.1f, 3, _slate.halfHP));
                    break;
                case 3:
                    NowCoroutine(_pattern.RandomMoveAttack(_slate, g_minimis, 10, 5, 2, 3, _slate.halfHP));
                    break;
                case 4:
                    NowCoroutine(_pattern.StopAndGoAttack(_slate, 15, 3, 1, 3, _slate.halfHP));
                    break;
            }
        }
    }
}
