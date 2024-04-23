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

        if (!sendDataList.ContainsKey(data.index))
        {
            sendDataList.Add(data.index, data);
        }
        else
        {
            sendDataList[data.index].Power = sendDataList[data.index].Power > data.Power ? sendDataList[data.index].Power : data.Power;
        }

    }

    public override void Attack(Transform target)
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
