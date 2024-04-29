using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPistol : Skill
{
    [SerializeField] private ExplosionBullet explosionBlt;

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        if (target == null) return;

        var obj = Instantiate(explosionBlt, weaponTrm.position, weaponTrm.rotation);
        obj.SetDamage(power * 10, power);

    }

}
