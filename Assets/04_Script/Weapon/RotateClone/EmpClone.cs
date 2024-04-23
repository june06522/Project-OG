using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpClone : RotateClone
{
    [SerializeField] EMPBomb empBomb;
    protected override void Attack()
    {
        Vector3 targetPos = transform.up * 2f;
        Instantiate(empBomb, transform.position, transform.rotation)
            .Throw(targetPos, damage: _DataSO.AttackDamage.GetValue());

        transform.DOScale(transform.localScale * 1.5f, 0.25f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutBounce);

    }
}
