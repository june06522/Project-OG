using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebugSword : InvenWeapon
{

    SpriteRenderer _spriteRenderer;

    [SerializeField] private Bullet attack;

    [SerializeField] private Bullet skill1;
    [SerializeField] private Bullet skill2;

    private bool isAttack = false;

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    [BindExecuteType(typeof(float))]
    public override void GetSignal([BindParameterType(typeof(float))] object signal)
    {
        if ((float)signal == 0)
        {
            Shoot(target, skill1);
        }
        else if ((float)signal == 1)
        {
            Shoot(target, skill2);
        }
    }

    private void Shoot(Transform target, Bullet prefab)
    {
        RotateWeapon(target);
        var blt = Instantiate(prefab, transform.position, transform.rotation);
        blt.Shoot(prefab.Data.Damage);

        //transform.DOShakePosition(0.1f, 0.25f);
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

    protected override void Attack(Transform target)
    {
        Shoot(target, attack);
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
