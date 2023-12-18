using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugWeapon : Weapon
{


    public override void Run(Transform target)
    {

        base.Run(target);

        Rotate(target);

    }

    private void Rotate(Transform target)
    {

        var dir = target.position - transform.position;

        transform.right = -dir.normalized;

    }

    protected override void Attack(Transform target)
    {

        Debug.Log("Attack");

    }

    protected override void Skill(int count)
    {

        Debug.Log($"Skill : {count}");

    }

}
