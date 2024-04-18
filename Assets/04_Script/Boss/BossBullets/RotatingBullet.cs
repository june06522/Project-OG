using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBullet : BossBullet
{
    private Boss _boss;
    private float _deg = 0;
    [SerializeField]
    private float _speed;


    protected override void OnEnable()
    {
        _boss = GetComponentInParent<Boss>();
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

        if (_boss.IsDie)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.BossBulletType5, this.gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
