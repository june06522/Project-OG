using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseState
{
    protected Boss _boss;

    protected bool _willChange = false;

    protected BossBaseState(Boss boss)
    {
        _boss = boss;
    }

    public abstract void OnBossStateOn();

    public abstract void OnBossStateUpdate();

    public abstract void OnBossStateExit();

    public virtual IEnumerator RandomPattern(float waitTime)
    {
        yield return null;
    }
}
