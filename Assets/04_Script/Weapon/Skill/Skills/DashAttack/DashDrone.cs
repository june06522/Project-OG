using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDrone : Skill
{

    [SerializeField] private Bullet bullet;

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {

        var blt = Instantiate(bullet, weaponTrm.position, weaponTrm.rotation);
        blt.Shoot(bullet.Data.Damage * power);
        blt.transform.localScale = Vector3.one * 2f;

        transform.DOShakePosition(0.1f, 0.25f);

    }

}
