using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBullet : BossBullet
{
    private float _deg;
    [SerializeField]
    private float _speed;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void Update()
    {
        if(isRotateBullet)
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
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
