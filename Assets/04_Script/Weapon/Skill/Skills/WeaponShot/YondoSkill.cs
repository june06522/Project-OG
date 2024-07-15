using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class YondoSkill : Skill
{
    [SerializeField] SwordYondo yondo;
    [Header("ETC")]
    [SerializeField] LayerMask layerMask;
    [SerializeField]
    float minLifeTime = 3f;
    [SerializeField]
    float radius = 50f;

    [SerializeField]
    float minDamage = 3f;

    private int curInstCount = 1;
    private float curDamage;
    private float curLifeTime;


    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.F))
        //{
        //    Excute(transform, null, 5);
        //}
    }

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        CurPowerInit(power);
        if (Physics2D.OverlapCircle(weaponTrm.parent.position, radius, layerMask))
        {
            SwordYondo obj = Instantiate(yondo, weaponTrm.parent.position, weaponTrm.rotation);
            obj.Init(layerMask, power, radius, curLifeTime, curDamage);   
        }
    }

    public override void Power1()
    {
        curInstCount = 1;
        curDamage = minDamage;
        curLifeTime = minLifeTime;

        isMaxPower = false;
    }

    public override void Power2()
    {
        curInstCount = 1;
        curDamage = minDamage * 2f;
        curLifeTime = minLifeTime + 1f;

        isMaxPower = false;
    }

    public override void Power3()
    {
        curInstCount = 1;
        curDamage = minDamage * 3f;
        curLifeTime = minLifeTime + 2f;

        isMaxPower = false;
    }

    public override void Power4()
    {
        curInstCount = 1;
        curDamage = minDamage * 4f;
        curLifeTime = minLifeTime + 3f;


        isMaxPower = false;
    }

    public override void Power5()
    {
        curInstCount = 1;
        curDamage = minDamage * 5f;
        curLifeTime = minLifeTime + 4f;

        isMaxPower = true;
    }
}
