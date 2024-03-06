using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttackState : BombRootState
{
    public BombAttackState(BombStateController controller) : base(controller)
    {

    }

    protected override void EnterState()
    {
        controller.StopImmediately();
        float randomTime = Random.Range(1f,2f);
        Attack(randomTime);
    }

    private void Attack(float randomTime)
    {
        Sequence seq = DOTween.Sequence();

        Tween shakeTween =
            controller.transform.DOShakeScale(randomTime);
        Color startColor = Color.white;
        Color endColor = Color.red;

        Tween twinkleTween = 
            DOTween.To(() => startColor, cur => controller.ChangeColor(cur, false), endColor, randomTime / 6).SetLoops(3, LoopType.Yoyo) ;

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
