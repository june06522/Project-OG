using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FDeadState : BossBaseState
{
    private FlowerPattern _pattern;
    private FlowerBoss _flower;

    public FDeadState(FlowerBoss boss, FlowerPattern pattern) : base(boss, pattern)
    {
        _flower = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        _flower.gameObject.layer = LayerMask.NameToLayer("Default");
        _flower.ReturnAll();
        _flower.ReturnFlowerCollector();
        _flower.StartCoroutine(Dying(2));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator Dying(int disappearingTime)
    {
        _flower.StartCoroutine(_pattern.FlowerDeadShot(_flower, 5, 5, 3, 10, 5, 0.5f));
        yield return new WaitForSeconds(0.5f * 4);

        _flower.StartCoroutine(ActiveFalse(_flower.gameObject, disappearingTime));
        _flower.StartCoroutine(ActiveFalse(_flower.bigestBody, disappearingTime));
        _flower.StartCoroutine(ActiveFalse(_flower.mediumSizeBody, disappearingTime));
        _flower.StartCoroutine(ActiveFalse(_flower.smallestBody, disappearingTime));
    }
}
