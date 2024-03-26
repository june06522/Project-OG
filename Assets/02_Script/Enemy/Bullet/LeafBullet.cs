using System;
using UnityEngine;
using DG.Tweening;

public class LeafBullet : EnemyBullet
{
    [SerializeField]
    private float dissolveTime;
    Material material;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        //material = spriteRenderer.material;

        Init();
    }

    public void Init()
    {
        //material.SetFloat("_SourceGlowDissolveFade", 0);
    }

    public void Make(Transform targetTrm, Vector3 makePos)
    {
       
        float fadeValue = 0;
        DOTween.To(() => fadeValue,
            (value) => material.SetFloat("_SourceGlowDissolveFade", value), 1, dissolveTime)
            .OnComplete(() => Shoot((targetTrm.position - transform.position).normalized));
    }
}
