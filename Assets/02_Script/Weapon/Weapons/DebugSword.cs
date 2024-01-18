using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSword : InvenWeapon
{

    SpriteRenderer _spriteRenderer;

    [SerializeField] private Bullet attack;

    [SerializeField] private Bullet skill1;
    [SerializeField] private Bullet skill2;


    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    [BindExecuteType(typeof(float))]
    public override void GetSignal([BindParameterType(typeof(float))] object signal)
    {
        if ((float)signal == 0)
        {

        }
        else if ((float)signal == 1)
        {

        }
    }

    private void Shoot(Transform target, Bullet prefab)
    {
        var blt = Instantiate(prefab, transform.position, transform.rotation);
        blt.Shoot(prefab.Data.Damage);

        transform.DOShakePosition(0.1f, 0.25f);
    }

    protected override void Attack(Transform target)
    {
        Shoot(target, attack);
    }

    protected override void RotateWeapon(Transform target)
    {

        if (target == null) return;

        var dir = target.position - transform.position;
        dir.Normalize();
        dir.z = 0;

        _spriteRenderer.flipY = dir.x switch
        {

            var x when x > 0 => false,
            var x when x < 0 => true,
            _ => _spriteRenderer.flipY

        };

        transform.right = dir;

    }

}
