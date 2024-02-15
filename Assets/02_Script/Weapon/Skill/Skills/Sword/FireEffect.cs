using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class FireEffect : MonoBehaviour
{
    [SerializeField] private BulletDataSO data;
    float damage;

    List<IHitAble> hitAbles = new List<IHitAble>();

    public BulletData Data { get; protected set; }

    SpriteRenderer _renderer;

    private void Init(float value)
    {
        _renderer = GetComponent<SpriteRenderer>();
        Data = data.CreateBulletData();
        Destroy(gameObject, value);
        _renderer.DOFade(0, value);

    }

    public void Shoot(float damage, float lifeTime)
    {

        this.damage = damage;
        Init(lifeTime);

    }

    private void Tick(float damage, float time, Transform hitable)
    {
        var a = hitable.AddComponent<DamageOverTick>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<IHitAble>(out var hitable))
        {


            if (!hitAbles.Contains(hitable))
            {
                Tick(damage, 3, collision.transform);

            }

        }

    }
}
