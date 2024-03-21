using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBulletTest : BossBullet
{
    [SerializeField]
    private float f_speed;
    private float f_deg;
    private float f_s;

    private bool b_x;
    private bool b_y;

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        f_deg = f_s = 0;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    void Update()
    {
        f_deg += 0.01f * f_speed;
        f_s = Mathf.Sin(f_deg) * 0.01f;

        if (Mathf.Abs(rigid.velocity.x) > Mathf.Abs(rigid.velocity.y))
        {
            transform.Translate(new Vector3(0, f_s, 0));
        }
        else
        {
            transform.Translate(new Vector3(f_s, 0, 0));
        } 
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
