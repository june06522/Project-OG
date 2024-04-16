using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseState
{
    private Boss _boss;

    protected IEnumerator co;

    protected BossBaseState(Boss boss, BossPatternBase pattern)
    {
        _boss = boss;
    }

    public abstract void OnBossStateOn();

    public abstract void OnBossStateUpdate();

    public abstract void OnBossStateExit();

    // This Function was created to know what pattern is using now, please use this for patterns coroutine
    public void NowCoroutine(IEnumerator coroutine)
    {
        co = coroutine;
        _boss.StartCoroutine(co);
    }

    public void StopNowCoroutine()
    {
        _boss.isAttacking = false;
        if(co != null)
            _boss.StopCoroutine(co);
    }

    protected IEnumerator ActiveFalse(GameObject obj, float disappearingTime)
    {
        float curTime = 0;
        float a = 1;
        while (curTime < disappearingTime)
        {
            curTime += Time.deltaTime;
            if (a > 0)
            {
                obj.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, a -= Time.deltaTime * disappearingTime);
            }
            yield return null;
        }
        obj.SetActive(false);
    }
}
