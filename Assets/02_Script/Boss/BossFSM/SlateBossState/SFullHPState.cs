using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFullHPState : BossBaseState
{
    private SlateBoss _slate;
    private Quaternion rot;

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
        for (int i = 0; i < 4; i++)
        {
            GameObject minimi = ObjectPool.Instance.GetObject(ObjectPoolType.SlateMinimi, _boss.transform);
            minimi.transform.position = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / 4), Mathf.Sin(Mathf.PI * 2 * i / 4));
            minimi.transform.rotation = Quaternion.identity;
        }
    }

    private IEnumerator RandomPattern(float waitTime)
    {
        Debug.Log("laser");
        yield return new WaitForSeconds(waitTime);

        int rand = 1;// Random.Range(1, 5);

        switch(rand)
        {
            case 1:
                NowCoroutine(WarningLine(10, 100));
                break;
        }
    }

    // 미니미 레이저 추가해야함
    private IEnumerator WarningLine(float warningTime, float speed)
    {
        GameObject warning = ObjectPool.Instance.GetObject(ObjectPoolType.WarningType0, _slate.G_slateOnlyCollector.transform);
        warning.transform.position = _slate.transform.position;

        yield return null;

        _slate.StartCoroutine(TurnLine(false, warning, warningTime, speed, ObjectPoolType.WarningType0));
    }

    private IEnumerator Laser()
    {
        GameObject laser = ObjectPool.Instance.GetObject(ObjectPoolType.Laser, _slate.G_slateOnlyCollector.transform);
        laser.transform.position = _slate.transform.position;
        laser.transform.rotation = rot;
        yield return null;
    }

    private IEnumerator TurnLine(bool isLaser, GameObject obj, float turnTime, float speed, ObjectPoolType type)
    {
        float curTime = 0;
        float angle = 0;
        float deg = 0;
        bool isDown = false;
        Vector3 startDir = Vector3.zero;
        if (_slate.transform.position.x > GameManager.Instance.player.transform.position.x)
        {
            startDir = Vector3.back;
        }
        else if (_slate.transform.position.x < GameManager.Instance.player.transform.position.x)
        {
            startDir = Vector3.forward;
        }

        if (_slate.transform.position.y >= GameManager.Instance.player.transform.position.y)
        {
            isDown = true;
        }
        else if (_slate.transform.position.y < GameManager.Instance.player.transform.position.y)
        {
            isDown = false;
        }

        while (curTime < turnTime)
        {
            curTime += Time.deltaTime;
            deg += Time.deltaTime * speed;
            if (deg < 360)
            {
                float rad = Mathf.Deg2Rad * (deg + 360);

                float x = Mathf.Cos(rad);

                float y = 0;
                if (isDown)
                {
                    y = -Mathf.Sin(rad);
                }
                else
                {
                    y = Mathf.Sin(rad);
                }


                angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

                obj.transform.rotation = Quaternion.AngleAxis(angle + 90, startDir);
            }
            else
            {
                deg = 0;
            }

            yield return null;
        }

        yield return null;

        if(!isLaser)
        {
            rot = obj.transform.rotation;
            ObjectPool.Instance.ReturnObject(type, obj);
            _slate.StartCoroutine(Laser());
        }
        else
        {
            ObjectPool.Instance.ReturnObject(type, obj);
        }
    }
}
