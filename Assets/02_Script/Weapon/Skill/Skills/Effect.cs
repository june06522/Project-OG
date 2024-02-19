using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    [SerializeField] private BulletDataSO data;
    float damage;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<IHitAble>(out var hitable))
        {

            if (collision.CompareTag("Player"))
            {

            }
            else
            {

                hitable.Hit(damage);

            }

        }

    }

}
