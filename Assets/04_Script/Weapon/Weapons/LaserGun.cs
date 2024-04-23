using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : InvenWeapon
{
    [SerializeField] Transform _shootPos;
    [SerializeField] LaserGunLine gunLine;

    SpriteRenderer _spriteRenderer;


    protected override void Awake()
    {

        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public override void Attack(Transform target)
    {

        var obj = Instantiate(gunLine, Vector3.zero, Quaternion.identity);

        obj.LineRenderer.positionCount = 2;
        Debug.Log(_shootPos.localPosition);
        RaycastHit2D hit = Physics2D.Raycast(_shootPos.position, _shootPos.right, int.MaxValue, LayerMask.GetMask("Wall"));

        if (hit.collider != null)
        {
            obj.SetLine(_shootPos.position, hit.point, Data.AttackDamage.GetValue());
            obj.LineRenderer.enabled = true;
            obj.EdgeCollider.SetPoints(new List<Vector2>
            {
                _shootPos.position,
                hit.point
            });

        }


        DOTween.To(() => obj.LineRenderer.widthMultiplier, x => obj.LineRenderer.widthMultiplier = x, 0f, 0.5f)
            .OnComplete(() =>
            {
                Destroy(obj, 0.1f);
            });

    }



    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {

        //var data = (SendData)signal;
        //SkillContainer.Instance.GetSKill((int)id, (int)data.GeneratorID)?.Excute(transform, target, data.Power);

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
