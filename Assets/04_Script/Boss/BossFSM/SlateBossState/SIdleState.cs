using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SIdleState : BossBaseState
{
    private SlateBoss _slate;

    public SIdleState(SlateBoss boss, BossPatternBase pattern) : base(boss, pattern)
    {
        _slate = boss;
    }

    public override void OnBossStateExit()
    {
        _slate.SetBody(_slate.bigestBody, Vector3.one, Vector3.zero, _slate.bossColor, 0.5f);
        _slate.SetBody(_slate.mediumSizeBody, Vector3.one, Vector3.zero, _slate.bossColor, 0.5f);
        _slate.SetBody(_slate.smallestBody, Vector3.one, Vector3.zero, _slate.bossColor, 0.5f);
    }

    public override void OnBossStateOn()
    {
        _slate.gameObject.layer = LayerMask.NameToLayer("Default");
        _slate.StartCoroutine(IdleAnimation(0.5f));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator IdleAnimation(float animTime)
    {
        _slate.bigestBody.transform.DORotate(new Vector3(0, 0, 45), animTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _slate.mediumSizeBody.transform.DORotate(new Vector3(0, 0, 45), animTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _slate.smallestBody.transform.DORotate(new Vector3(0, 0, 45), animTime)
                    .SetEase(Ease.InOutSine);
                });
            });

        yield return new WaitForSeconds(animTime * 3);

        _slate.bigestBody.transform.DORotate(new Vector3(0, 0, 0), animTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _slate.mediumSizeBody.transform.DORotate(new Vector3(0, 0, 0), animTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _slate.smallestBody.transform.DORotate(new Vector3(0, 0, 0), animTime)
                    .SetEase(Ease.InOutSine);
                });
            });

        yield return new WaitForSeconds(animTime * 3);

        _slate.isIdle = false;
    }
}
