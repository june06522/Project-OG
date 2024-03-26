using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollision : MonoBehaviour
{

    private float damage;
    List<IHitAble> hits = new List<IHitAble>();
    public void SetDamage(float damage)
    {

        this.damage = damage;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")) return;

        if (collision.TryGetComponent<IHitAble>(out var h))
        {
            if (hits.Contains(h)) return;

            h.Hit(damage);
            h.Hit(damage * 1.2f);
            h.Hit(damage * 1.5f);
            hits.Add(h);
            
        }

    }

}
