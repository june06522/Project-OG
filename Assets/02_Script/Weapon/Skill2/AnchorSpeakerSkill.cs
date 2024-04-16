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

        obj.transform.DOJump(target.transform.position, 5f, 1, Vector2.Distance(transform.position, target.transform.position));
        
        obj.StartCoroutine(obj.Attack(3 + power));
    }
}
