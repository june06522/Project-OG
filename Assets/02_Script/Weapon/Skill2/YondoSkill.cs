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
    float coolTime = 5f;
    [SerializeField]
    float radius = 50f;

    private int curInstCount = 1;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Excute(transform, null, 5);
        }
    }

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        CurPowerInit(power);
        if(Physics2D.OverlapCircle(weaponTrm.parent.position, radius, layerMask))
        {
            for(int i = 0; i < curInstCount; i++)
            {
                SwordYondo obj = Instantiate(yondo, weaponTrm.parent.position, weaponTrm.rotation);
                obj.Init(layerMask, power, radius, coolTime);
            }
        }
    }

    public override void Power1()
    {
        curInstCount = 1;

        isMaxPower = false;
    }

    public override void Power2()
    {
        curInstCount = 1;

        isMaxPower = false;
    }

    public override void Power3()
    {
        curInstCount = 2;

        isMaxPower = false;
    }

    public override void Power4()
    {
        curInstCount = 2;

        isMaxPower = false;
    }

    public override void Power5()
    {
        curInstCount = 5;
        isMaxPower = true;
    }
}
