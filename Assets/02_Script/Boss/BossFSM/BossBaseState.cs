using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseState
{
    protected Boss _boss;

    protected IEnumerator co;

    protected BossBaseState(Boss boss)
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
        if(co != null)
            _boss.StopCoroutine(co);
    }

    public void ChangeMat(int index = 0)
    {
        _boss.MyMat = _boss.mats[index];
    }
}
