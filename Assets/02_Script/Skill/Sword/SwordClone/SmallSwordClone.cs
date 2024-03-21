using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSwordClone : SwordClone
{
    //public override void Attack(Vector3 targetPos)
    //{
    //    IsAttack = true;
    //    Sequence seq;
        
    //    seq.Append(transform.DORotate(new Vector3(0, 0, angle), 0.2f));
    //    seq.Insert(0, transform.DOMove(transform.position + -dir, 0.5f));
    //}

    public override void CheckHit(Vector2 targetPos)
    {
        base.CheckHit(targetPos);
    }

    public override void Setting(float dissolveTime, float width, float height)
    {
        base.Setting(dissolveTime, width, height);
    }
}
