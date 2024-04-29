using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSword : Skill
{

    [SerializeField] GameObject slash;

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        float angle = Mathf.Atan2(weaponTrm.right.y, weaponTrm.right.x) * Mathf.Rad2Deg;

        var temp = Instantiate(slash, weaponTrm.position, Quaternion.Euler(0, 0, angle - 90));

    }
}
