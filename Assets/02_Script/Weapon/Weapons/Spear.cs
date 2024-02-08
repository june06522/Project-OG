using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : InvenWeapon
{

    SpriteRenderer _spriteRenderer;

    protected override void Awake()
    {

        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }


    public override void GetSignal(object signal)
    {

        var a = (int)signal;
        SkillManager.Instance.GetSKill((int)id, a)?.Excute(transform, target);

    }

    protected override void Attack(Transform target)
    {

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
