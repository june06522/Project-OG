using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpClone : RotateClone
{
    [SerializeField] EMPBomb empBomb;
    Vector2 originScale;

    private void Start()
    {
        originScale = transform.localScale;    
    }

    public override void Attack(Transform targetTrm)
    {
        transform.DOKill();
        transform.localScale = originScale;

        Vector3 targetPos = transform.position + transform.right * 2f;
        Instantiate(empBomb, transform.position, transform.rotation)
            .Throw(targetPos, damage: Data.GetDamage());

        transform.DOScale(transform.localScale * 1.5f, 0.25f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutBounce);

    }
}
