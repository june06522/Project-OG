using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBulletTest : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private float deg;
    private float _s;

    private bool _x;
    private bool _y;

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        deg = _s = 0;
    }

    void Update()
    {
        deg += 0.01f * _speed;
        _s = Mathf.Sin(deg) * 0.01f;

        if (Mathf.Abs(rigid.velocity.x) > Mathf.Abs(rigid.velocity.y))
        {
            transform.Translate(new Vector3(0, _s, 0));
        }
        else
        {
            transform.Translate(new Vector3(_s, 0, 0));
        } 
    }
}
