using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : InvenWeapon
{
    GameObject visual;
    [SerializeField] private float _stingBackTime = 0.2f;
    public bool _isAttack = false;

    SpriteRenderer _spriteRenderer;

    protected override void Awake()
    {

        base.Awake();
        visual = transform.GetChild(0).gameObject;
        _spriteRenderer = visual.GetComponent<SpriteRenderer>();

    }

    public override void Run(Transform target)
    {
        base.Run(target);

        if (!_isAttack)
        {

            visual.transform.position = transform.position;

        }

    }

    public override void GetSignal(object signal)
    {

        var a = (int)signal;
        SkillManager.Instance.GetSKill((int)id, a)?.Excute(visual.transform, target);

    }

    protected override void Attack(Transform target)
    {

        if (!_isAttack)
        {

            StartCoroutine(Sting(target));

        }

    }


    private IEnumerator Sting(Transform trm)
    {
        _isAttack = true;
        Vector3 startPosition = visual.transform.position;
        Vector3 endPosition = trm.position;

        float elapsedTime = 0f;

        while (elapsedTime < _stingBackTime)
        {
            visual.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / _stingBackTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        visual.transform.position = endPosition;
        _isAttack = false;
        //visual.transform.localPosition = this.startPosition;
    }

    protected override void RotateWeapon(Transform target)
    {

        if (target == null) return;

        var dir = target.position - visual.transform.position;
        dir.Normalize();
        dir.z = 0;

        _spriteRenderer.flipY = dir.x switch
        {

            var x when x > 0 => false,
            var x when x < 0 => true,
            _ => _spriteRenderer.flipY

        };

        visual.transform.right = dir;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("HitAble"))
        {

            collision.GetComponent<IHitAble>().Hit(Data.AttackDamage.GetValue());

        }

    }

}
