using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

// ±ÇÃÑ
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


    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {

        var data = (SendData)signal;
        SkillContainer.Instance.GetSKill((int)id, (int)data.GeneratorID)?.Excute(transform, target, data.Power);

        //var skill = Instantiate(SkillManager.Instance.GetSKill((int)id, (int)data.GeneratorID));

        //skill.Excute(transform, target, data.Power);
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
