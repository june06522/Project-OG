using DG.Tweening;
using UnityEngine;

public class Drone : InvenWeapon
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

        SkillManager.Instance.RegisterSkill(data.TriggerID,this,data);

    }

    public override void Attack(Transform target)
    {

        var blt = Instantiate(bullet, shootPos.position, transform.rotation);
        blt.Shoot(Data.GetDamage());

        if (_attackSoundClip != null)
        {

            SoundManager.Instance.SFXPlay("AttackSound", _attackSoundClip, 0.5f);

        }
        transform.DOShakePosition(0.1f, 0.25f);

    }

    protected override void RotateWeapon(Transform target)
    {

        if (target == null)
        {
            transform.rotation = Quaternion.identity;   
            _spriteRenderer.flipY = false;
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
