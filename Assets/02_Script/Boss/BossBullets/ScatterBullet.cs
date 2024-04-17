using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBullet : BossBullet
{
    private GameObject bulletCollector;

    private Rigidbody2D rigid;

    private float _deg;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float angle;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float time;
    [SerializeField]
    private float damage;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        bulletCollector = GameObject.Find("BulletCollector");
    }

    private void Update()
    {
        _deg += Time.deltaTime * _speed;

        if (_deg < 360)
        {
            transform.rotation = Quaternion.Euler(0, 0, _deg * -1);
        }
        else
        {
            _deg = 0;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Scatter());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private IEnumerator Scatter()
    {
        Rigidbody2D[] rigids = new Rigidbody2D[2];

        while(true)
        {
            yield return new WaitForSeconds(time);

            for (int j = 0; j < 2; j++)
            {
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, bulletCollector.transform);
                bullet.GetComponent<BossBullet>().Attack(damage);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;
                rigids[j] = bullet.GetComponent<Rigidbody2D>();
            }

            rigids[0].velocity = Quaternion.Euler(0, 0, angle) * rigid.velocity.normalized * speed;
            rigids[1].velocity = Quaternion.Euler(0, 0, -angle) * rigid.velocity.normalized * speed;
        }
    }
}
