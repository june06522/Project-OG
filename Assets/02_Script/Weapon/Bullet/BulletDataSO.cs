using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BulletData
{

    public float Speed;
    public float Damage;
    public string[] HitAbleTag;

}

[CreateAssetMenu(menuName = "SO/Bullet/Data")]
public class BulletDataSO : ScriptableObject
{

    public float Speed;
    public float Damage;
    public string[] HitAbleTag;

    public BulletData CreateBulletData()
    {

        return new BulletData
        {

            Speed = Speed,
            Damage = Damage,
            HitAbleTag = HitAbleTag

        };

    }

}