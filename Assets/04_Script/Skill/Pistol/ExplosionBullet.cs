using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : MonoBehaviour
{
    [SerializeField]
    private float damage;
    private float power = 1;
    [SerializeField] private float speed;
    [SerializeField] ParticleSelfDestroyer explosion;
    [SerializeField] private LayerMask targetLayer;

    public void SetDamage(float damage, float power)
    {
        this.damage = damage;
        this.power = power;
    }

    private void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag != "Player")
            if (collision.TryGetComponent<IHitAble>(out var hit))
            {

                hit.Hit(damage);
                var obj = Instantiate(explosion, transform.position, Quaternion.identity);
                obj.transform.localScale = Vector3.one * power;

                Destroy(gameObject);

            }

    }

}
