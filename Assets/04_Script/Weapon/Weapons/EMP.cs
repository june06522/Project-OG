using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EMP : InvenWeapon
{
    SpriteRenderer _spriteRenderer;
    private bool isAttack = false;

    [SerializeField] EMPBomb empBomb;

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

    }

    public override void Attack(Transform target)
    {
        isAttack = true;

        Vector3 targetPos = target.position;
        if (_attackSoundClip != null)
        {

            SoundManager.Instance.SFXPlay("AttackSound", _attackSoundClip, 0.5f);

        }

        Instantiate(empBomb, transform.position, transform.rotation)
            .Throw(targetPos, damage: Data.AttackDamage.GetValue(), 0f, transform.localScale.x);

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

        if (!sendDataList.ContainsKey(data.index))
        {
            sendDataList.Add(data.index, data);
        }
        else
        {
            sendDataList[data.index].Power = sendDataList[data.index].Power > data.Power ? sendDataList[data.index].Power : data.Power;
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

    public override void Run(Transform target)
    {
        base.Run(target);

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
