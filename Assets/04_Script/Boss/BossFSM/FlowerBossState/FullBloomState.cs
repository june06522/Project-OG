using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullBloomState : BossBaseState
{
    private FlowerPattern _pattern;
    private FlowerBoss _flower;

    public FullBloomState(FlowerBoss boss, FlowerPattern pattern) : base(boss, pattern)
    {
        _flower = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        _flower.SetBodyToBasic(_flower.bigestbody, _flower.bigestBody);
        _flower.SetBodyToBasic(_flower.mediumsizebody, _flower.mediumSizeBody);
        _flower.SetBodyToBasic(_flower.smallestbody, _flower.smallestBody);
        _flower.fullBloom = false;
        _flower.isAttacking = false;
    }

    public override void OnBossStateOn()
    {
        Debug.Log("FullBloom");
        _flower.fullBloom = true;
        NowCoroutine(_pattern.FullBloomPattern(_flower, 2, 100));
    }

    public override void OnBossStateUpdate()
    {
        if(!_flower.fullBloom)
        {
            StopNowCoroutine();
        }
    }
}
