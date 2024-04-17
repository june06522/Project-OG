using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDeadState : BossBaseState
{
    private SlateBoss _slate;
    public SDeadState(SlateBoss boss, SlatePattern pattern) : base(boss, pattern)
    {
        _slate = boss;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        _slate.gameObject.layer = LayerMask.NameToLayer("Default");
        _slate.StopAllCoroutines();
        _slate.ReturnAll();
        _slate.LaserReturnAll();
        _slate.StartCoroutine(Dying(2));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator Dying(float disappearingTime)
    {
        yield return null;
        SoundManager.Instance.SFXPlay("Dead", _slate.deadClip, 1);
        _slate.StartCoroutine(ActiveFalse(_slate.gameObject, disappearingTime));
        _slate.StartCoroutine(ActiveFalse(_slate.bigestBody, disappearingTime));
        _slate.StartCoroutine(ActiveFalse(_slate.mediumSizeBody, disappearingTime));
        _slate.StartCoroutine(ActiveFalse(_slate.smallestBody, disappearingTime));
    }
}
