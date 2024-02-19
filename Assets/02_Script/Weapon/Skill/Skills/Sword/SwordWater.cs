using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWater : Skill
{

    [SerializeField] WaterEffect water;

    public override void Excute(Transform weaponTrm, Transform target, int power, Guid guid)
    {
        
        var w = Instantiate(water, weaponTrm.position, Quaternion.identity);
        Debug.Log(power);
        w.Shoot(water.Data.Damage * power, power * 2, GameManager.Instance.player);

    }

}
