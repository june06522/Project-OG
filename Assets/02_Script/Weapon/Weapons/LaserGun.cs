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
        Instantiate(gunLine, Vector3.zero, Quaternion.identity);

        gunLine.LineRenderer.positionCount = 2;
        Debug.Log(_shootPos.localPosition);
        RaycastHit2D hit = Physics2D.Raycast(_shootPos.position, _shootPos.right, int.MaxValue, LayerMask.GetMask("Wall"));

        if (hit.collider != null)
        {
            gunLine.SetLine(_shootPos.position, hit.point); 
            gunLine.LineRenderer.enabled = true;
            gunLine.EdgeCollider.SetPoints(new List<Vector2>
            {
                _shootPos.position,
                hit.point
            });

        }


        //DOTween.To(() => gunLine.LineRenderer.widthMultiplier , x => gunLine.LineRenderer.widthMultiplier  = x, 0f, 3f)
        //    .OnComplete(() =>
        //    {
        //        gunLine.LineRenderer.positionCount = 0;

        //    });

    }



    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {

        //var data = (SendData)signal;
        //SkillContainer.Instance.GetSKill((int)id, (int)data.GeneratorID)?.Excute(transform, target, data.Power);

        var data = (SendData)signal;

        if (!sendDataList.ContainsKey(data.GetHashCode()))
        {
            sendDataList.Add(data.GetHashCode(), data);
        }
        else
        {
            sendDataList[data.GetHashCode()].Power = sendDataList[data.GetHashCode()].Power > data.Power ? sendDataList[data.GetHashCode()].Power : data.Power;
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
