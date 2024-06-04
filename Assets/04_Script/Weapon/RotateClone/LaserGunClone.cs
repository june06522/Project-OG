using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserGunClone : RotateClone
{

    [SerializeField] Transform _shootPos;
    [SerializeField] LaserGunLine gunLine;

    protected override void Attack()
    {

        var obj = Instantiate(gunLine, transform);

        obj.LineRenderer.positionCount = 2;
        
        RaycastHit2D hit = Physics2D.Raycast(_shootPos.position, _shootPos.right, int.MaxValue, LayerMask.GetMask("Wall"));

        if (hit.collider != null)
        {
            obj.SetLine(_shootPos.position, hit.point, Data.GetDamage());
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
}
