using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDeadState : BossBaseState
{
    private SlateBoss _slate;
    public SDeadState(SlateBoss boss) : base(boss)
    {
        _slate = boss;
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
