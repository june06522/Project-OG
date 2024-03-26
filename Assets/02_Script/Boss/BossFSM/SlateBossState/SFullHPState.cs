using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFullHPState : BossBaseState
{
    private SlateBoss _slate;
    private GameObject[] g_minimis = new GameObject[4]; 

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
        for (int i = 0; i < 8; i++)
        {
            if(i % 2 != 0)
            {
                g_minimis[i / 2] = ObjectPool.Instance.GetObject(ObjectPoolType.SlateMinimi, _boss.transform);
                g_minimis[i / 2].transform.position = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / 8), Mathf.Sin(Mathf.PI * 2 * i / 8)).normalized * _slate.F_minimiAwayDistance;
                g_minimis[i / 2].gameObject.name = (i / 2).ToString();
                g_minimis[i / 2].transform.rotation = Quaternion.identity;
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
                NowCoroutine(Laser(2, 100));
                break;
        }
    }

    // 레이저 라인 랜더러로 바꾸기
    private IEnumerator Laser(float warningTime, float fireTime)
    {
        float curTime = 0;
        GameObject warning = ObjectPool.Instance.GetObject(ObjectPoolType.WarningType0, _slate.G_slateOnlyCollector.transform);
        warning.transform.position = _slate.transform.position;

        while(curTime < warningTime)
        {
            curTime += Time.deltaTime;
            warning.transform.Rotate(Vector3.forward);
            yield return null;
        }

        ObjectPool.Instance.ReturnObject(ObjectPoolType.WarningType0, warning);
        GameObject[] minimiLasers = new GameObject[g_minimis.Length];

        for(int i = 0; i < g_minimis.Length; i++)
        {
            minimiLasers[i] = ObjectPool.Instance.GetObject(ObjectPoolType.SlateMinimiLaser, _slate.G_slateOnlyCollector.transform);
            minimiLasers[i].GetComponent<BossBullet>().Attack(_slate.bossSo.Damage);
            minimiLasers[i].transform.position = g_minimis[i].transform.position;
            minimiLasers[i].transform.rotation = Quaternion.identity;
        }
        GameObject laser = ObjectPool.Instance.GetObject(ObjectPoolType.Laser, _slate.G_slateOnlyCollector.transform);
        laser.GetComponentInChildren<BossBullet>().Attack(_slate.bossSo.Damage);
        laser.transform.position = _slate.transform.position;
        laser.transform.rotation = warning.transform.rotation;
        curTime = 0;

        while(curTime < fireTime)
        {
            curTime += Time.deltaTime;
            laser.transform.Rotate(Vector3.forward);
            for(int i = 0; i < g_minimis.Length; i++)
            {
                if(i == 0 || i == 3)
                {
                    minimiLasers[i].transform.Rotate(Vector3.back * Time.deltaTime * 20);
                }
                else
                {
                    minimiLasers[i].transform.Rotate(Vector3.forward * Time.deltaTime * 20);
                }
            }
            yield return null;
        }

        ObjectPool.Instance.ReturnObject(ObjectPoolType.Laser, laser);
        for(int i = 0; i < g_minimis.Length; i++)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.SlateMinimiLaser, minimiLasers[i]);
        }
    }
}
