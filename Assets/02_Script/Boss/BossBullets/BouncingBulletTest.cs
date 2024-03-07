using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBulletTest : BossBullet
{
    private Rigidbody2D _rigid;

    public LayerMask _mask;

    RaycastHit2D rHit;
    RaycastHit2D lHit;
    RaycastHit2D uHit;
    RaycastHit2D dHit;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        Vector2 dir = _rigid.velocity;

        rHit = Physics2D.Raycast(transform.position, Vector2.right, 1, _mask);
        lHit = Physics2D.Raycast(transform.position, Vector2.left, 1, _mask);
        uHit = Physics2D.Raycast(transform.position, Vector2.up, 1, _mask);
        dHit = Physics2D.Raycast(transform.position, Vector2.down, 1, _mask);

        if ((rHit.collider != null || lHit.collider != null) && (uHit.collider != null || dHit.collider != null))
            _rigid.velocity = new Vector2(-dir.x, -dir.y);
        else if (rHit.collider != null || lHit.collider != null)
            _rigid.velocity = new Vector2(-dir.x, dir.y);
        else if (uHit.collider != null || dHit.collider != null)
            _rigid.velocity = new Vector2(dir.x, -dir.y);
    }
}
