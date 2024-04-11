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

    protected override void HitOther()
    {
        if (curHitCount < maxHitCount)
        {
            curHitCount++;
        }
        else
        {
            Release();
        }
    }
}
