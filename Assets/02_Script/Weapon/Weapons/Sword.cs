using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 테스트용
public class Sword : InvenWeapon
{

    SpriteRenderer _spriteRenderer;
    Collider2D _col;
    private bool isAttack = false;

    protected override void Awake()
    {

        base.Awake();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _col = transform.GetChild(0).GetComponent<Collider2D>();

    }


    [BindExecuteType(typeof(float))]
    public override void GetSignal([BindParameterType(typeof(float))] object signal)
    {

        var data = (SendDataSO)signal;
        SkillManager.Instance.GetSKill((int)id, (int)data.GeneratorID)?.Excute(transform, target, data.Power);

    }

    protected override void Attack(Transform target)
    {

        //RotateWeapon(target);

        DOTween.Sequence().
            Append(transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z - 60), 0)).
            Append(transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z + 60), 0.1f).SetEase(Ease.Linear)).
            //Append(transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z - 60), 0.1f).SetEase(Ease.Linear)).
            Append(transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z), 0.1f).SetEase(Ease.Linear));


        AttackTween();

    }

    private IEnumerator AttackTween()
    {

        isAttack = true;
        _col.enabled = true;
        yield return new WaitForSeconds(0.35f);
        _col.enabled = false;
        isAttack = false;

    }

    public override void Run(Transform target)
    {
        base.Run(target);

        if (!isAttack)
        {

            _col.transform.localPosition = new Vector3(1, 0, 0);

        }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("HitAble"))
        {

            collision.GetComponent<IHitAble>().Hit(Data.AttackDamage.GetValue());

        }

    }

}
