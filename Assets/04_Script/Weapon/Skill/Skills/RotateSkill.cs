using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkill : Skill
{
    [SerializeField] WeaponID _ID;

    RotateSkillManager rotateManager;

    private void Awake()
    {
        rotateManager = FindObjectOfType<RotateSkillManager>();
    }

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        CurPowerInit(power);

        rotateManager.SetCloneInfo(weaponTrm.GetComponent<Weapon>(), _ID);      
    }


    //Power Init
    public override void Power1()
    {
    }

    public override void Power2()
    {
    }

    public override void Power3()
    {
    }

    public override void Power4()
    {
    }

    public override void Power5()
    {
    }


}
