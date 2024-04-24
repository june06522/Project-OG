using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashLaser : Skill
{

    [SerializeField] LaserGunLine gunLine;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        var obj = Instantiate(gunLine, Vector3.zero, Quaternion.identity);

        obj.LineRenderer.positionCount = 2;

        RaycastHit2D hit = Physics2D.Raycast(weaponTrm.position, weaponTrm.right, int.MaxValue, LayerMask.GetMask("Wall"));

        if (hit.collider != null)
        {

            obj.SetLine(weaponTrm.position, hit.point, power * 10, 0.5f + power * 0.5f);
            obj.LineRenderer.enabled = true;
            obj.EdgeCollider.SetPoints(new List<Vector2>
            {
                weaponTrm.position,
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
