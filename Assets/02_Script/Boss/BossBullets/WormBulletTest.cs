using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBulletTest : BossBullet
{
    [SerializeField]
    private float _angle;
    [SerializeField]
    private float _time;

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Movement());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private IEnumerator Movement()
    {
        while(true)
        {
            rigid.velocity = Quaternion.Euler(0, 0, _angle) * rigid.velocity;

            yield return new WaitForSeconds(_time);

            rigid.velocity = Quaternion.Euler(0, 0, -_angle) * rigid.velocity;

            yield return new WaitForSeconds(_time);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
