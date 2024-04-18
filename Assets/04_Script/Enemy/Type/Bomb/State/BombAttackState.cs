using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombAttackState : NormalRootState
{
    private new BombStateController controller;

    public BombAttackState(BombStateController controller) : base(controller)
    {
        this.controller = controller;
    }

    protected override void EnterState()
    {
        controller.InstantiateRange();
        controller.StopImmediately();
        float randomTime = 0.5f;
        Attack(randomTime);
    }

    private void Attack(float randomTime)
    {
        DG.Tweening.Sequence seq = DOTween.Sequence();

        Tween shakeTween =
            controller.transform.DOShakeScale(randomTime + 0.1f, strength:0.4f, vibrato:20, randomness: 40, false, ShakeRandomnessMode.Harmonic);
        Color startColor = Color.white;
        Color endColor = Color.red;

        Tween twinkleTween = 
            DOTween.To(() => startColor, cur => controller.ChangeColor(cur, false), endColor, randomTime / 4).SetLoops(4, LoopType.Yoyo).SetEase(Ease.InOutCirc) ;
        Tween bombTween =
            controller.transform.DOMoveY(transform.position.y + 1f, 0.5f);

        seq.Append(shakeTween);
        seq.Insert(0, twinkleTween);
        seq.Insert(0f, bombTween);
        seq.OnComplete(() =>
        {
            controller.Boom();
        });
        seq.Play();
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

    }
}
