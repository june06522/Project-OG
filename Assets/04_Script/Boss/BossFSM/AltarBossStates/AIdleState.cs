using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIdleState : BossBaseState
{
    private AltarBoss _boss;

    public AIdleState(AltarBoss boss, AltarPattern pattern) : base(boss, pattern)
    {
        _boss = boss;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        _boss.StartCoroutine(StartAnimation(0.5f));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator StartAnimation(float animTime)
    {
        _boss.smallestBody.transform.DOMove(new Vector3(0.6f, 0.6f, 0), animTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _boss.smallestBody.transform.DOMove(new Vector3(-0.6f, 0.6f, 0), animTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _boss.smallestBody.transform.DOMove(new Vector3(0, -0.6f, 0), animTime)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        _boss.isIdleEnded = true;
                    });
                });
            });

        yield return null;
    }
}
