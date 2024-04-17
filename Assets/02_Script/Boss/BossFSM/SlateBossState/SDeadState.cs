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
        _slate.GetComponent<SpriteRenderer>().sprite = _slate.deadSprite;
        _slate.StopAllCoroutines();
        _slate.ReturnAll();
        _slate.LaserReturnAll();
        _slate.StartCoroutine(Dying(2, 3, 0.5f));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator Dying(float disappearingTime, int explosionCount, float explosionWaitTime)
    {
        SoundManager.Instance.SFXPlay("Dead", _slate.deadClip, 1);
        for(int i = 0; i < explosionCount; i++)
        {
            GameObject effect = ObjectPool.Instance.GetObject(ObjectPoolType.SlateDeadEffect);
            effect.transform.position = _slate.transform.position;

            yield return new WaitForSeconds(explosionWaitTime);
        }
        _slate.StartCoroutine(ActiveFalse(_slate.gameObject, disappearingTime));
    }
}
