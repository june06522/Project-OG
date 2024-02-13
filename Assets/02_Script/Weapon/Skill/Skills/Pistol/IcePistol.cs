using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IcePistol : Skill
{
    [SerializeField] Bullet iceBullet;
    [SerializeField] float damage;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        var a = Instantiate(iceBullet, weaponTrm.position, weaponTrm.rotation);

        a.Shoot(damage * power);

    }

}
