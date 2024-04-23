using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaborDashSkill : Skill
{
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;

    private Vector2 targetScale;
    private Sword sword;
    
    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        CurPowerInit(power);
        
        if(sword == null)
            sword = weaponTrm.GetComponent<Sword>();

        sword.transform.DOKill();
        sword.transform
            .DOScale(targetScale, duration)
            .SetEase(ease)
            .OnComplete( () => sword.ReinforceAttack(target));
    }

    public override void Power1()
    {
        targetScale = Vector2.one * 1.5f;
    }

    public override void Power2()
    {
        targetScale = Vector2.one * 2f;
    }

    public override void Power3()
    {
        targetScale = Vector2.one * 2.5f;
    }

    public override void Power4()
    { 
        targetScale = Vector2.one * 3f;
    }

    public override void Power5()
    {
        targetScale = Vector2.one * 5f;
    }
}
