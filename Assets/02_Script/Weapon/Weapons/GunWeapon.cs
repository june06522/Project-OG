using DG.Tweening;
using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : Weapon
{

    [SerializeField] private Transform shootPos;
    [SerializeField] private string bulletKey = "DefaultBullet";

    private SpriteRenderer spriteRenderer;
    private bool isShake;

    protected override void Awake()
    {
        base.Awake();
        
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public override void Run(Transform target)
    {

        FlipAndRotate(target);

        base.Run(target);

    }

    private void FlipAndRotate(Transform target)
    {

        var dir = target.position - transform.position;
        dir.Normalize();
        dir.z = 0;

        spriteRenderer.flipY = dir.x switch
        {

            var x when x > 0 => false,
            var x when x < 0 => true,
            _ => spriteRenderer.flipY

        };

        transform.right = dir;

    }

    protected override void Attack(Transform target)
    {

        var blt = FAED.TakePool<Bullet>(bulletKey, shootPos.position, Quaternion.identity);
        FAED.TakePool("Muzzle", shootPos.position).transform.right = transform.right;

        blt.transform.right = transform.right;
        blt.Shoot(Data.WeaponValue.GetValue());

        if (!isShake)
        {

            isShake = true;

            transform
                .DOShakePosition(0.07f, 0.4f)
                .OnComplete(() => isShake = false);

        }

    }

    protected override void Skill(int count)
    {
    }

    public override void OnRePosition()
    {

        DOTween.Kill(transform);
        isShake = false;

    }

}
