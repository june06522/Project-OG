using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkill : Skill
{
    [SerializeField] WeaponID _ID;

    private int spawnCount;
    RotateSkillManager rotateManager;

    private void Awake()
    {
        rotateManager = FindObjectOfType<RotateSkillManager>();
    }

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        CurPowerInit(power);

        rotateManager.SetCloneInfo(_ID, power);
    }


    //Power Init
    public override void Power1()
    {
        spawnCount = 1;
        isMaxPower = false;
    }

    public override void Power2()
    {
        spawnCount = 2;
        isMaxPower = false;
    }

    public override void Power3()
    {
        spawnCount = 3;
        isMaxPower = false;
    }

    public override void Power4()
    {
        spawnCount = 4;
        isMaxPower = false;
    }

    public override void Power5()
    {
        spawnCount = 5;
        isMaxPower = true;
    }


}
