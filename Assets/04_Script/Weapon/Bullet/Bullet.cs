using FD.Dev;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private BulletDataSO data;

    protected float curDamage;
    protected bool isAdd;

    public BulletData Data { get; set; }

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
        Release();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAdd) return;

        foreach (var item in Data.HitAbleTag)
        {

            if (collision.CompareTag(item))
            {

                HitOther();

                if (collision.TryGetComponent<IHitAble>(out var hitAble))
                {

                    hitAble.Hit(curDamage + Data.Damage);

                }

            }

        }

    }

    private IEnumerator ReleaseBulletCo()
    {

        yield return new WaitForSeconds(8f);
        Release();

    }

}