using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDeadState : BossBaseState
{
    private CrabBoss _crab;

    private CrabPattern _pattern;

    private float _waitTime;

    public CDeadState(CrabBoss boss, CrabPattern pattern) : base(boss, pattern)
    {
        _crab = boss;
        _pattern = pattern;
        _waitTime = 1f;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        _crab.gameObject.layer = LayerMask.NameToLayer("Default");
        _crab.StartCoroutine(DieAnimation(_waitTime));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator DieAnimation(float waitTime)
    {
        float currentTime = 0;
        float a = 1;
        float speed = a / waitTime;
        while(currentTime < waitTime)
        {
            currentTime += Time.deltaTime;
            a -= Time.deltaTime * speed;

            foreach(var sprite in _crab.spriteRendererList)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, a);
            }
            yield return null;
        }
        _crab.gameObject.SetActive(false);
    }
}
