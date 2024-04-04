using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FDeadState : BossBaseState
{
    public FDeadState(Boss boss, BossPatternBase pattern) : base(boss, pattern)
    {
    }

    public override void OnBossStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnBossStateOn()
    {
        throw new System.NotImplementedException();
    }

    public override void OnBossStateUpdate()
    {
        throw new System.NotImplementedException();
    }
}
