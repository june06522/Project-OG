using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIdleState : BossBaseState
{
    private CrabBoss _crab;

    private CrabPattern _pattern;

    private float _animationTime;

    public CIdleState(CrabBoss boss, CrabPattern pattern) : base(boss, pattern)
    {
        _crab = boss;
        _pattern = pattern;
        _animationTime = 1f;
    }

    public override void OnBossStateExit()
    {
        _crab.animator.SetBool(_crab.start, false);
    }

    public override void OnBossStateOn()
    {
        _crab.idleAnimationEnd = false;
        _crab.animator.speed = 1;
        _crab.animator.SetBool(_crab.start, true);
    }

    public override void OnBossStateUpdate()
    {
        
    }
}
