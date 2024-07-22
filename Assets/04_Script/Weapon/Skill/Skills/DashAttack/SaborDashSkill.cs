using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaborDashSkill : Skill
{
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;

    private float scalefactor;
    private Sword sword;

    Coroutine coroutine;
    
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        CurPowerInit(power);
        
        sword = weaponTrm.GetComponent<Sword>();
        
        if (sword != null)
            sword.StartReinforceAttack(target, scalefactor);

        sword = null;
    }

    public override void Power1()
    {
        scalefactor = 1.5f;
    }

    public override void Power2()
    {
        scalefactor = 2f;
    }

    public override void Power3()
    {
        scalefactor = 2.5f;
    }

    public override void Power4()
    { 
        scalefactor = 3f;
    }

    public override void Power5()
    {
        scalefactor = 5f;
    }
}
