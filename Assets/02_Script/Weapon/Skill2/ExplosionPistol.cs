using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPistol : Skill
{
    [SerializeField] private ExplosionBullet explosionBlt;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        if (target == null) return;

        var obj = Instantiate(explosionBlt, weaponTrm.position, Quaternion.LookRotation(weaponTrm.right));
        obj.SetDamage(power * 10, power);

    }

}
