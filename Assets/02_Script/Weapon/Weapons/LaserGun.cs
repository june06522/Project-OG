using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : InvenWeapon
{
    [SerializeField] LineRenderer _line;
    [SerializeField] Transform _shootPos;
    SpriteRenderer _spriteRenderer;

    protected override void Awake()
    {

        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public override void Attack(Transform target)
    {
        Physics2D.Raycast(_shootPos.position, _shootPos.right,);
    }

    public override void GetSignal(object signal)
    {
        throw new System.NotImplementedException();
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
