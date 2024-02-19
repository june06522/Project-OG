using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : InvenWeapon
{
    GameObject visual;
    [SerializeField] ExtraSpear extra;
    [SerializeField] private float _stingBackTime = 0.2f;

    public bool _isAttack = false;
    float elapsedTime = 0;
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

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {

        var data = (SendData)signal;
        SkillContainer.Instance.GetSKill((int)id, (int)data.GeneratorID)?.Excute(transform, target, data.Power, WeaponGuid);

    }

    public override void Attack(Transform target)
    {

        if (!_isAttack)
        {

            StartCoroutine(Sting(target));

        }

    }

    public void AttackImmediately()
    {
        elapsedTime = float.MaxValue;
        Attack(target);
    }

    private IEnumerator Sting(Transform trm)
    {
        _isAttack = true;
        Vector3 startPosition = visual.transform.position;
        Vector3 endPosition = trm.position;

        elapsedTime = 0f;

        while (elapsedTime < _stingBackTime)
        {
            visual.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / _stingBackTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trm.GetComponent<IHitAble>().Hit(Data.AttackDamage.GetValue());
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
            Debug.Log(1);
            collision.GetComponent<IHitAble>().Hit(Data.AttackDamage.GetValue());

        }

    }

}
