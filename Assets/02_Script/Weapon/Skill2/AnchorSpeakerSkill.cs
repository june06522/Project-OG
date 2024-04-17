using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorSpeakerSkill : Skill
{
    [SerializeField] AnchorSpeaker speaker;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        var obj = Instantiate(speaker, transform.position, Quaternion.identity);

        DOTween.Sequence()
            .Append(obj.transform.DOJump(target.transform.position, 2f, 1, 0.8f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                obj.StartCoroutine(obj.Attack(3 + power));
            });

    }
}
