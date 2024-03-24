using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBullet : BossBullet
{
    private void Update()
    {
        if (GetComponentInParent<Boss>().B_isDead)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.BossBulletType5, this.gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
