using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttackState : BombRootState
{
    Action act;
    public BombAttackState(BombStateController controller) : base(controller)
    {
        act = controller.InstantiateRange;
    }

    protected override void EnterState()
    {
        act.Invoke();
        controller.StopImmediately();
        float randomTime = UnityEngine.Random.Range(1f,2f);
        Attack(randomTime);
    }

    private void Attack(float randomTime)
    {
        Sequence seq = DOTween.Sequence();

        Tween shakeTween =
            controller.transform.DOShakeScale(randomTime, strength:0.3f, vibrato:20, randomness: 40, false);
        Color startColor = Color.white;
        Color endColor = Color.red;

        Tween twinkleTween = 
            DOTween.To(() => startColor, cur => controller.ChangeColor(cur, false), endColor, randomTime / 6).SetLoops(6, LoopType.Yoyo) ;

        seq.Append(shakeTween);
        seq.Insert(0, twinkleTween);
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
