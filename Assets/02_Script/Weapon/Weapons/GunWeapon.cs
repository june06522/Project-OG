using DG.Tweening;
using FD.Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class GunWeapon : Weapon
{

    [SerializeField] private Transform shootPos;
    [SerializeField] private string bulletKey = "DefaultBullet";

    private SpriteRenderer spriteRenderer;
    private LineRenderer laserRanderer;
    private float laserDuration;
    private float laserAttackCool = 0.1f;
    private float lastLaserAttack;
    private bool isShake;

    protected override void Awake()
    {
        base.Awake();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerController.EventController.OnDash += HandleChargeDash;

    }

    protected virtual void HandleChargeDash()
    {

        Charge(10);

    }

    public override void Run(Transform target)
    {

        FlipAndRotate(target);

        base.Run(target);

        LaserSkill(target);
    }

    private void LaserSkill(Transform target)
    {

        if(target == null && laserRanderer != null)
        {

            laserRanderer.SetPosition(0, Vector2.zero);
            laserRanderer.SetPosition(1, Vector2.zero);
            return;

        }

        if (laserDuration <= 0)
        {

            if(laserRanderer != null)
            {

                FAED.InsertPool(laserRanderer.gameObject);
                laserRanderer = null;

            }

            return;

        }

        laserDuration -= Time.deltaTime;

        laserRanderer.SetPosition(0, shootPos.position);
        laserRanderer.SetPosition(1, target.position);

        if(Time.time - lastLaserAttack >= laserAttackCool)
        {

            lastLaserAttack = Time.time;
            target.GetComponent<IHitAble>().Hit(Data.WeaponValue.GetValue());

        }

    }

    private void FlipAndRotate(Transform target)
    {

        if (target == null) return;

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

        laserDuration += 3 * count;

        if(laserRanderer == null)
        {

            laserRanderer = FAED.TakePool<LineRenderer>("Laser");

        }

    }

    public override void OnRePosition()
    {

        DOTween.Kill(transform);
        isShake = false;

    }

    private void OnDestroy()
    {

        PlayerController.EventController.OnDash -= HandleChargeDash;

        if (laserRanderer != null)
        {

            Destroy(laserRanderer.gameObject);

        }

    }

}
