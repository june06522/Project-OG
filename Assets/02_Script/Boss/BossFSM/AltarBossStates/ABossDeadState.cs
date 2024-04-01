using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABossDeadState : BossBaseState
{
    private AltarBoss _altarBoss;

    public ABossDeadState(AltarBoss boss) : base(boss)
    {
        _altarBoss = boss;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        _boss.gameObject.layer = LayerMask.NameToLayer("Default");
        _boss.StopAllCoroutines();
        _boss.ReturnAll();
        _altarBoss.ChainReturnAll();
        NowCoroutine(Dying(3, 2, 0.5f));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator Dying(float dyingEffectTime, float disappearingTime, float disappearSpeed)
    {
        float curTime = 0;
        float a = 1;
        SoundManager.Instance.SFXPlay("Dead", _altarBoss.audios[3], 1);
        _altarBoss.ChangeMat(4);
        yield return new WaitForSeconds(dyingEffectTime);
        GameObject effect = ObjectPool.Instance.GetObject(ObjectPoolType.AltarDeadEffect);
        effect.transform.position = _boss.transform.position;
        _altarBoss.ChangeMat(5);
        while(curTime < disappearingTime)
        {
            curTime += Time.deltaTime;
            if (a > 0)
            {
                _boss.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, a -= Time.deltaTime * disappearSpeed);
            }
            yield return null;
        }
        _boss.gameObject.SetActive(false);
    }


}
