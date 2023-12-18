using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Bullet : MonoBehaviour
{

    [SerializeField] private BulletDataSO data;

    private float curDamage;
    private bool isAdd;

    public BulletData Data { get; protected set; }

    private void Awake()
    {

        Data = data.CreateBulletData();

    }

    public void Shoot(float weaponDamage)
    {

        BulletJobManager.Instance.AddBullet(this);
        isAdd = true;
        curDamage = weaponDamage + Data.Damage;

        StartCoroutine(ReleaseBulletCo());

    }

    public void Release()
    {

        if (BulletJobManager.Instance.RemoveBullet(this))
        {

            FAED.InsertPool(gameObject);

        }
        else
        {

            Destroy(gameObject);

        }

        isAdd = false;

        StopAllCoroutines();

    }

    protected virtual void HitOther()
    {
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

        if (!isAdd) return;

        foreach (var item in Data.HitAbleTag)
        {

            if (collision.CompareTag(item))
            {

                HitOther();
                Release();

                if(collision.TryGetComponent<IHitAble>(out var hitAble))
                {

                    hitAble.Hit(curDamage + Data.Damage);

                }

            }

        }

    }

    private IEnumerator ReleaseBulletCo()
    {

        yield return new WaitForSeconds(20f);
        Release();

    }

}