using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraSpear : MonoBehaviour
{

    [SerializeField] public float Damage;

    public void Shoot(Transform target, float damage)
    {

        DOTween.Sequence().
            Append(transform.DOJump(target.position, 1.5f, 1, 0.5f).SetEase(Ease.Linear)).
            AppendCallback(() =>
            {
                Destroy(gameObject);
                target.GetComponent<IHitAble>().Hit(damage);
            });
        
    }

}
