using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneClone : RotateClone
{
    [SerializeField] private Transform shootPos;
    [SerializeField] private Bullet bullet;

    protected override void Attack()
    {

        var blt = Instantiate(bullet, shootPos.position, transform.rotation);
        blt.Shoot(bullet.Data.Damage);

        transform.DOShakePosition(0.1f, 0.25f);

    }

}
