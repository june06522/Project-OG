using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHammer : Skill
{

    [SerializeField] GameObject hammerEffect;
 
    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        float angle = Mathf.Atan2(weaponTrm.right.y, weaponTrm.right.x) * Mathf.Rad2Deg;

        var temp = Instantiate(hammerEffect, weaponTrm.position, Quaternion.Euler(0, 0, angle - 90));

        temp.GetComponent<OrbCollision>().SetDamage(power * 10);
        temp.GetComponent<Bullet>().Shoot(1);
        Destroy(temp, 1.6f);
    }

}
