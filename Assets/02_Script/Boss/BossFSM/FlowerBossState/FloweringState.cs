using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloweringState : BossBaseState
{
    private FlowerPattern _pattern;
    private FlowerBoss _flower;

    public FloweringState(FlowerBoss boss, FlowerPattern pattern) : base(boss, pattern)
    {
        _pattern = pattern;
        _flower = boss;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        // 나중에 패턴에 옮기셈
        //NowCoroutine(_pattern.FlowerShapeShot(_flower, 6, 3, 2, 10, 5, 1, false));
        //NowCoroutine(_pattern.ScatterShot(_flower, 6, 5, 3, 45, 1));
        //NowCoroutine(_pattern.RandomOminidirShot(_flower, 2, 20, 3, 1));
        //NowCoroutine(_pattern.WarmShot(_flower, 5, 5, 3, 0.1f));
        //NowCoroutine(_pattern.FullBloomPattern(_flower, 2, 1, 100, 10));
    }

    public override void OnBossStateUpdate()
    {
        
    }
}
