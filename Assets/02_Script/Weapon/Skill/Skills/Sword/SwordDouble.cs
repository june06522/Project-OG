using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDouble : Skill
{

    WeaponController controller;

    private void Awake()
    {

        controller = GameManager.Instance.player.GetComponent<WeaponController>();

    }

    public override void Excute(Transform weaponTrm, Transform target, int power, Guid guid)
    {

        controller.FindWeapon(guid).Attack(controller.FindCloseEnemy(10));

    }

}
