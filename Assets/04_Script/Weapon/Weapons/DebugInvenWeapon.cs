using DG.Tweening;
using UnityEngine;

public class DebugInvenWeapon : InvenWeapon
{

    [SerializeField] private Transform _shootPos;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Bullet skill1;
    [SerializeField] private Bullet skill2;

    SpriteRenderer _spriteRenderer;

    protected override void Awake()
    {

        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public override void Attack(Transform target)
    {

        Shoot(target, _bullet);

    }

    private void Shoot(Transform target, Bullet prefab)
    {

        var blt = Instantiate(prefab, _shootPos.position, transform.rotation);
        blt.Shoot(prefab.Data.Damage);

        transform.DOShakePosition(0.1f, 0.25f);

    }

    [BindExecuteType(typeof(Skill))]
    public override void GetSignal([BindParameterType(typeof(Skill))] object signal)
    {

        // 여기 바꿔야함
        //Debug.Log(signal);

        if ((float)signal == 0f)
        {
            Shoot(target, skill1);
        }
        else if ((float)signal == 1f)
        {
            Shoot(target, skill2);
        }

    }


    protected override void RotateWeapon(Transform target)
    {
        if (target == null)
        {
            transform.rotation = Quaternion.identity;
            return;
        }

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
