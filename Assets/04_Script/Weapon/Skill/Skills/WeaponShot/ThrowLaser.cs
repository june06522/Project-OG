using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowLaser : Skill
{
    [SerializeField] Bullet laserBullet;

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {

        var obj = Instantiate(laserBullet, weaponTrm.position, weaponTrm.rotation);
        obj.Shoot(power * 3);

    }

}
