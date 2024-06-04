using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerClone : RotateClone
{
    [SerializeField] AudioClip clip;
    [SerializeField] SpeakerAttack _attack;

    public override void Attack(Transform targetTrm)
    {
        DOTween.Sequence().
            Append(transform.DOScale(Vector2.one * 1.3f, 0.2f).SetEase(Ease.InBounce)).
            Append(transform.DOScale(Vector2.one, 0.2f));

        SoundManager.Instance?.SFXPlay("Hammer", clip);
        StartCoroutine(AttackTween());

    }

    private IEnumerator AttackTween()
    {

        yield return new WaitForSeconds(0.2f);
        var obj = Instantiate(_attack, transform.position, Quaternion.identity);
        obj.SetDamage(Data.GetDamage());
        yield return new WaitForSeconds(0.2f);

    }
}
