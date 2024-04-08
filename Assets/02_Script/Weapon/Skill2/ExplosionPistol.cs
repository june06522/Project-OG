using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPistol : Skill
{
    [SerializeField] private ExplosionBullet explosionBlt;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        var obj = Instantiate(explosionBlt, transform.position, transform.rotation);
        obj.SetDamage(power * 10, power);

    }

}