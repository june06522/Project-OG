using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SDeadState : BossBaseState
{
    private SlateBoss _slate;

    private SlatePattern _pattern;

    private Material _mat;

    private SpriteRenderer _spriteRenderer;

    public SDeadState(SlateBoss boss, SlatePattern pattern) : base(boss, pattern)
    {
        _slate = boss;
        _pattern = pattern;
        _spriteRenderer = _slate.smallestBody.GetComponent<SpriteRenderer>();
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        _mat = _spriteRenderer.material;

        _slate.gameObject.layer = LayerMask.NameToLayer("Default");

        _slate.StopAllCoroutines();
        _slate.ReturnAll();
        _slate.LaserReturnAll();
        _slate.StartCoroutine(Dying(2, 200));
        _slate.StartCoroutine(ActiveFalse(_slate.bigestBody, 2));
        _slate.StartCoroutine(ActiveFalse(_slate.mediumSizeBody, 2));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator Dying(float disappearingTime, float animTime)
    {
        _mat.SetFloat("_VibrateFade", 1);
        _mat.SetFloat("_VibrateOffset", 0.5f);

        yield return null;

        _slate.StartCoroutine(_pattern.LastLaserAttack(_slate, _slate.line, _slate.transform.position, animTime, 0.1f));

        yield return new WaitForSeconds(360 / (animTime - 10));

        _mat.SetFloat("_VibrateFade", 0);
        _mat.SetFloat("_VibrateOffset", 0.1f);
        SoundManager.Instance.SFXPlay("Dead", _slate.deadClip, 1);
        _slate.StartCoroutine(ActiveFalse(_slate.gameObject, disappearingTime));
        _slate.StartCoroutine(ActiveFalse(_slate.smallestBody, disappearingTime));
    }
}
