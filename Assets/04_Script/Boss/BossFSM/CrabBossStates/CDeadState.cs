using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDeadState : BossBaseState
{
    private CrabBoss _crab;

    private CrabPattern _pattern;

    public CDeadState(CrabBoss boss, CrabPattern pattern) : base(boss, pattern)
    {
        _crab = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        Debug.Log("Dead");
    }

    public override void OnBossStateUpdate()
    {
        
    }
}
