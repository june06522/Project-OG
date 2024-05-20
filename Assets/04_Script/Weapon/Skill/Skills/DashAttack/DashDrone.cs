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
        blt.GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);
        blt.Shoot(bullet.Data.Damage * power * power);
        blt.transform.localScale = Vector3.one * 4f;

        transform.DOShakePosition(0.1f, 0.25f);

    }

}
