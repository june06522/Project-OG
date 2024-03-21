using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossBaseState
{
    public BossIdleState(Boss boss) : base(boss)
    {
    }

    public override void OnBossStateExit()
    {

    }

    public override void OnBossStateOn()
    {
        _boss.transform.position = _boss.originPos;
        _boss.StopImmediately(_boss.transform);
    }

    public override void OnBossStateUpdate()
    {
    }
}
