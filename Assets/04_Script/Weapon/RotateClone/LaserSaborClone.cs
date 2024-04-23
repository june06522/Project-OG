using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSaborClone : RotateClone
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        IHitAble hitAble;
        if (collision.TryGetComponent<IHitAble>(out hitAble))
        {
            //SoundManager.Instance.SFXPlay("HitHammerRotate", _clip, 0.25f);
            //if (Frozen)
            //{
            //    IDebuffReciever debuffReciever;
            //    if (collision.TryGetComponent<IDebuffReciever>(out debuffReciever))
            //    {
            //        debuffReciever.SetDebuff(EDebuffType.Frozen, 2f);
            //        debuffReciever.DebuffEffect(EDebuffType.Frozen, 2f);
            //    }
            //}

            hitAble.Hit(_DataSO.AttackDamage.GetValue());
        }
    }
}
