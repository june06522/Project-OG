using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraAttack : Skill
{

    [SerializeField] ExtraSpear extra;

    public override void Excute(Transform weaponTrm, Transform target, int power, InvenWeapon guid)
    {

        weaponTrm.GetComponent<Spear>().AttackImmediately();
        if (target == null) return;
        var e = Instantiate(extra, weaponTrm.position, weaponTrm.rotation);
        e.Shoot(target, extra.Damage * power);

    }

}
