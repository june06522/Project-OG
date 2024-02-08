using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 테스트용
public class DebugSword : InvenWeapon
{

    SpriteRenderer _spriteRenderer;

    [SerializeField] private Bullet _bullet;

    private bool isAttack = false;

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    [BindExecuteType(typeof(float))]
    public override void GetSignal([BindParameterType(typeof(float))] object signal)
    {

        var a = (int)signal;
        SkillManager.Instance.GetSKill((int)id, a)?.Excute(transform, target);

    }

    protected override void Attack(Transform target)
    {

        RotateWeapon(target);
        var blt = Instantiate(_bullet, transform.position, transform.rotation);
        blt.Shoot(_bullet.Data.Damage);

        transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z - 60), 0);
        transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z + 60), 0.25f);
        AttackTween();

    }

    private IEnumerator AttackTween()
    {

        isAttack = true;
        yield return new WaitForSeconds(0.35f);
        isAttack = false;

    }

    protected override void RotateWeapon(Transform target)
    {

        if (target == null) return;
        if (isAttack == true) return;

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
