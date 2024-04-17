using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorSpeakerSkill : Skill
{
    [SerializeField] AnchorSpeaker speaker;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        var obj = Instantiate(speaker, weaponTrm.position, Quaternion.identity);
        Vector3 point;
        if (target == null)
            point = (Vector2)weaponTrm.position + Random.insideUnitCircle * 2;
        else
            point = target.position;

        DOTween.Sequence()
            .Append(obj.transform.DOJump(point, 2f, 1, 0.8f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                obj.StartCoroutine(obj.Attack(3 + power));
            });

    }
}
