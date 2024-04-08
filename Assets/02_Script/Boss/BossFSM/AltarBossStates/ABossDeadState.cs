using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABossDeadState : BossBaseState
{
    private AltarBoss _altar;
    private Material mat;
    private SpriteRenderer sprite;

    public ABossDeadState(AltarBoss boss, AltarPattern pattern) : base(boss, pattern)
    {
        _altar = boss;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        _altar.gameObject.layer = LayerMask.NameToLayer("Default");
        sprite = _altar.GetComponent<SpriteRenderer>();
        _altar.StopAllCoroutines();
        _altar.ReturnAll();
        _altar.ChainReturnAll();
        NowCoroutine(Dying(3, 2, 0.5f));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator Dying(float dyingEffectTime, float disappearingTime, float disappearSpeed)
    {
        float curTime = 0;
        float a = 1;
        SoundManager.Instance.SFXPlay("Dead", _altar.deadClip, 1);
        _altar.ChangeMaterial(_altar.dyingMat);
        mat = _altar.dyingMat;
        mat.SetFloat("_VibrateFade", 1);
        yield return new WaitForSeconds(dyingEffectTime);
        GameObject effect = ObjectPool.Instance.GetObject(ObjectPoolType.AltarDeadEffect);
        effect.transform.position = _altar.transform.position;
        _altar.ChangeMaterial(_altar.deadMat);
        mat = sprite.material;
        while (curTime < disappearingTime)
        {
            curTime += Time.deltaTime;
            if (a > 0)
            {
                _altar.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, a -= Time.deltaTime * disappearSpeed);
                mat.SetFloat("_FullDistortionFade", a);
            }
            yield return null;
        }
        _altar.gameObject.SetActive(false);
    }


}
