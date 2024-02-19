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

        Data = data.CreateBulletData();

        _renderer = GetComponent<SpriteRenderer>();
        _renderer.DOFade(1, 0.3f);

        Destroy(gameObject, value + 0.1f);

        DOTween.Sequence().
            AppendInterval(value - 0.2f).
            Append(_renderer.DOFade(0, 0.3f));


    }

    public void Shoot(float damage, float lifeTime)
    {

        this.damage = damage;
        Init(lifeTime);

    }

    private void Tick(float damage, float time, Transform hitable)
    {

        var a = hitable.AddComponent<DamageOverTick>();
        a.Init(damage, time);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(collision.name);

        if (collision.TryGetComponent<IHitAble>(out var hitable))
        {

            if (collision.CompareTag("Player"))
            {

            }
            else
            {

                Debug.Log("tick");
                Tick(damage, 3, collision.transform);

            }

        }

    }

}
