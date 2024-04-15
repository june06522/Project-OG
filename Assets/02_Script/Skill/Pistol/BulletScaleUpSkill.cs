using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScaleUpSkill : Skill
{
    [SerializeField] ScaleUpBullet _scaleUpBullet;
    [SerializeField] int power;
    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        //power = this.power;
        CurPowerInit(power);
        
        var obj = Instantiate(_scaleUpBullet, weaponTrm.position, weaponTrm.rotation);

        obj.Init(power * 10, power, power);
        obj.Shoot(power * 10);
    }
}
