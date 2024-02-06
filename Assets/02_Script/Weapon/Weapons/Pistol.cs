using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class Pistol : InvenWeapon
{

    [SerializeField] private Transform shootPos;
    [SerializeField] private Bullet bullet;
    SpriteRenderer _spriteRenderer;

    protected override void Awake()
    {

        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }


    [BindExecuteType(typeof(int))]
    public override void GetSignal([BindParameterType(typeof(int))] object signal)
    {

        var a = (int)signal;
        SkillManager.Instance.GetSKill((int)id, a).Excute(transform, target);

    }

    protected override void Attack(Transform target)
    {

        var blt = Instantiate(bullet, shootPos.position, transform.rotation);
        blt.Shoot(bullet.Data.Damage);

        transform.DOShakePosition(0.1f, 0.25f);

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
