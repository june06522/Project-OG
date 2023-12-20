using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGun : GunWeapon
{

    protected override void Awake()
    {
        
        base.Awake();
        PlayerController.EventController.OnDash -= HandleChargeDash;

    }

    protected override void Attack(Transform target)
    {

        base.Attack(target);
        Charge(10);

    }

}
