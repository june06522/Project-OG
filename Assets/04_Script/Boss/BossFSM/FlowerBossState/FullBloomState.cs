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
        _flower.fullBloom = false;
        _flower.isAttacking = false;

        _flower.SetBody(_flower.bigestBody, Vector3.one, Vector3.zero, _flower.bossColor, 0.5f);
        _flower.SetBody(_flower.mediumSizeBody, Vector3.one, Vector3.zero, _flower.bossColor, 0.5f);
        _flower.SetBody(_flower.smallestBody, Vector3.one, Vector3.zero, _flower.bossColor, 0.5f);
    }

    public override void OnBossStateOn()
    {
        _flower.gameObject.layer = LayerMask.NameToLayer("Boss");
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
