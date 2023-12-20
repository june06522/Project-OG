using DG.Tweening;
using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearWeapon : Weapon
{

    private Vector2 origin;

    protected override void Awake()
    {

        base.Awake();

        PlayerController.EventController.OnDash += OnDashHandle;

    }

    private void OnDashHandle()
    {

        Charge(10);

    }

    public override void Run(Transform target)
    {

        Rotate(target);

        base.Run(target);

    }

    private void Rotate(Transform target)
    {

        if (target == null || Data.isAttackCoolDown) return;

        var dir = target.position - transform.position;
        transform.up = dir;

    }

    protected override void Attack(Transform target)
    {

        var targetPos = transform.localPosition + transform.up * 1.5f;

        origin = transform.localPosition;

        Sequence seq = DOTween.Sequence(transform);
        seq.Append(transform.DOLocalMove(targetPos, 0.2f).SetEase(Ease.OutExpo));
        seq.AppendCallback(() =>
        {

            target.GetComponent<IHitAble>().Hit(Data.WeaponValue.GetValue());

        });
        seq.AppendInterval(0.1f);
        seq.Append(transform.DOLocalMove(origin, 0.2f).SetEase(Ease.OutExpo));
        
    }

    protected override void Skill(int count)
    {

        if (target == null) return;

        StartCoroutine(SpawnSpearCo(count * 3));

    }
            
    public override void OnRePosition()
    {

        DOTween.Kill(transform);

    }

    private IEnumerator SpawnSpearCo(int spawnCount)
    {

        while (spawnCount > 0)
        {

            if(target == null) yield break;

            FAED.TakePool<MiniSpear>("MiniSpear",
                transform.position + (Vector3)Random.insideUnitCircle.normalized)
                .SetUp(target);
            yield return new WaitForSeconds(0.1f);
            spawnCount--;

        }


    }

    private void OnDestroy()
    {

        PlayerController.EventController.OnDash -= OnDashHandle;

    }

}
