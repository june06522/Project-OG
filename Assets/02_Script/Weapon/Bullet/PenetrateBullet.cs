using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrateBullet : Bullet
{
    private int curHitCount;
    private int maxHitCount;
    public void Init(int penetrateCnt)
    {
        maxHitCount = penetrateCnt;
        curHitCount = 0;
        
        BulletData bulletData = Data;
        bulletData.Speed *= penetrateCnt * 0.5f;
        Data = bulletData; 
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(curHitCount < maxHitCount)
        {
            foreach (var item in Data.HitAbleTag)
            {

                if (collision.CompareTag(item))
                {
                    HitOther();
                    if (collision.TryGetComponent<IHitAble>(out var hitAble))
                    {

                        hitAble.Hit(curDamage + Data.Damage);

                    }

                }

            }
            curHitCount++;
        }
        else
            base.OnTriggerEnter2D(collision);
    }
}
