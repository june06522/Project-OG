using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaborDashSkill : Skill
{
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;

    private Vector3 targetScale;
    private Sword sword;

    Coroutine coroutine;
    
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        CurPowerInit(power);
        
        if(sword == null)
            sword = weaponTrm.GetComponent<Sword>();
        
        Debug.Log("ReinForce");
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(sword.ReinforceAttack(target, targetScale));
    }

    public override void Power1()
    {
        targetScale = Vector3.one * 1.5f;
    }

    public override void Power2()
    {
        targetScale = Vector3.one * 2f;
    }

    public override void Power3()
    {
        targetScale = Vector3.one * 2.5f;
    }

    public override void Power4()
    { 
        targetScale = Vector3.one * 3f;
    }

    public override void Power5()
    {
        targetScale = Vector3.one * 5f;
    }
}
