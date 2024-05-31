using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EMP : InvenWeapon
{
    SpriteRenderer _spriteRenderer;
    private bool isAttack = false;

    [SerializeField] EMPBomb empBomb;

    Vector3 localScale;
    protected override void Awake()
    {

        base.Awake();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        localScale = transform.localScale;
    }

    public override void Attack(Transform target)
    {
        transform.DOKill();
        transform.localScale = localScale;

        isAttack = true;

        Vector3 targetPos = target.position;
        if (_attackSoundClip != null)
        {

            SoundManager.Instance.SFXPlay("AttackSound", _attackSoundClip, 0.5f);

        }

        Instantiate(empBomb, transform.position, transform.rotation)
            .Throw(targetPos, damage: Data.GetDamage(), 0f, transform.localScale.x);

        transform.DOScale(transform.localScale * 1.5f, 0.25f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutBounce);

        StartCoroutine(WaitAttackEnd());

    }

    private IEnumerator WaitAttackEnd()
    {
        yield return new WaitForSeconds(1f);
        isAttack = false;
    }

    [BindExecuteType(typeof(float))]
    public override void GetSignal([BindParameterType(typeof(float))] object signal)
    {

        var data = (SendData)signal;

        SkillManager.Instance.RegisterSkill(data.TriggerID, this, data);

    }

    protected override void RotateWeapon(Transform target)
    {
        if (target == null)
        {
            transform.rotation = Quaternion.identity;
            return;
        }
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

    public override void Run(Transform target, bool isSkill = false)
    {
        base.Run(target, isSkill);

        if (!isAttack)
        {

            transform.localPosition = origin;

        }

    }

    public override void OnRePosition()
    {
        origin = transform.localPosition;
    }

}
