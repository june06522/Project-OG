using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        CameraManager.Instance.CameraShake(10, 0.5f);
        _flower.gameObject.tag = "Untagged";
        _flower.gameObject.layer = LayerMask.NameToLayer("Default");

        _flower.ReturnAll();
        _flower.ReturnFlowerCollector();
        _flower.StartCoroutine(Dying(2, 0.5f));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator DieAnimation(float animTime)
    {
        _flower.bigestBody.transform.DORotate(new Vector3(0, 0, -180), animTime)
            .SetEase(Ease.InOutSine);
        _flower.mediumSizeBody.transform.DORotate(new Vector3(0, 0, 180), animTime)
            .SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(animTime);

        _flower.bigestBody.transform.DORotate(new Vector3(0, 0, 360), animTime)
        .SetEase(Ease.InOutSine)
        .OnComplete(() =>
        {
            _flower.bigestBody.transform.rotation = Quaternion.identity;
        });

        _flower.mediumSizeBody.transform.DORotate(new Vector3(0, 0, -360), animTime)
        .SetEase(Ease.InOutSine)
        .OnComplete(() =>
        {
            _flower.mediumSizeBody.transform.rotation = Quaternion.identity;
        });
    }

    private IEnumerator Dying(float disappearingTime, float animTime)
    {
        _flower.StartCoroutine(DieAnimation(animTime));
        yield return new WaitForSeconds(animTime); 
        _flower.StartCoroutine(ActiveFalse(_flower.bigestBody, disappearingTime));
        _flower.StartCoroutine(ActiveFalse(_flower.mediumSizeBody, disappearingTime));
        yield return new WaitForSeconds(disappearingTime);

        CameraManager.Instance.CameraShake(10, 0.5f);
        _flower.StartCoroutine(_pattern.FlowerDeadShot(_flower, 5, 7, 1, 10, 5, 0.5f));
        yield return new WaitForSeconds(0.5f);

        _flower.StartCoroutine(ActiveFalse(_flower.smallestBody, disappearingTime));
        _flower.StartCoroutine(ActiveFalse(_flower.gameObject, disappearingTime));
    }
}
