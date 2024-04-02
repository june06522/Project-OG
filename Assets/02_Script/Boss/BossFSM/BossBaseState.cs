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

    public void StopThisCoroutine()
    {
        _boss.isAttacking = false;
        if(co != null)
            _boss.StopCoroutine(co);
    }
}
