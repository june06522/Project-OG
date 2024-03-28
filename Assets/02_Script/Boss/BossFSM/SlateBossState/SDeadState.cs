using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDeadState : BossBaseState
{
    private SlateBoss _slate;
    public SDeadState(SlateBoss boss) : base(boss)
    {
        _slate = boss;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        _slate.gameObject.layer = LayerMask.NameToLayer("Default");
        _slate.GetComponent<SpriteRenderer>().sprite = _slate.L_sprite[2];
        _slate.StopAllCoroutines();
        _slate.ReturnAll();
        _slate.LaserReturnAll();
        _slate.StartCoroutine(Dying(2, 0.5f, 3, 0.5f));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator Dying(float disappearingTime, float disappearSpeed, int explosionCount, float explosionWaitTime)
    {
        float curTime = 0;
        float a = 1;
        for(int i = 0; i < explosionCount; i++)
        {
            GameObject effect = ObjectPool.Instance.GetObject(ObjectPoolType.SlateDeadEffect);
            effect.transform.position = _boss.transform.position;

            yield return new WaitForSeconds(explosionWaitTime);
        }
        
        while (curTime < disappearingTime)
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
