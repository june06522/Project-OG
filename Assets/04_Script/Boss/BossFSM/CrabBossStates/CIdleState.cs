using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIdleState : BossBaseState
{
    private CrabBoss _crab;

    private CrabPattern _pattern;

    public CIdleState(CrabBoss boss, CrabPattern pattern) : base(boss, pattern)
    {
        _crab = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        Debug.Log("Exit Idle");
    }

    public override void OnBossStateOn()
    {
        Debug.Log("Idle");
    }

    public override void OnBossStateUpdate()
    {
        
    }
}
