using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossBaseState
{
    private Boss _boss;
    public BossIdleState(Boss boss, BossPatternBase pattern) : base(boss, pattern)
    {
        _boss = boss;
    }

    public override void OnBossStateExit()
    {

    }

    public override void OnBossStateOn()
    {
        _boss.transform.localPosition = Vector3.zero;
        _boss.StopImmediately(_boss.transform);
    }

    public override void OnBossStateUpdate()
    {
    }
}
