using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePistol : Skill
{
    [SerializeField] Bullet fireBullet;
    [SerializeField] float damage;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        Debug.Log(12);

        var a = Instantiate(fireBullet, weaponTrm.position, weaponTrm.rotation);

        a.Shoot(power * power * damage);


    }

}
