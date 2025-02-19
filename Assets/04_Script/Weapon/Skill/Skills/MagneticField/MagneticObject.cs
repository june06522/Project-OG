using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    private float _damage;

    private List<IHitAble> enemiesOnTarget = new();

    public void SetDamage(float damage)
    {
        _damage = damage;

        Destroy(gameObject, 3);
        InvokeRepeating(nameof(Attack), 0.5f, 0.5f);
    }

    private void Attack()
    {
        for (int i = enemiesOnTarget.Count - 1; i >= 0; i--)
        {

            enemiesOnTarget[i]?.Hit(_damage);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;

        if (collision.TryGetComponent<IHitAble>(out var h))
        {

            enemiesOnTarget.Add(h);
            h.Hit(_damage);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHitAble>(out var h))
        {
            if (enemiesOnTarget.Contains(h))
            {
                enemiesOnTarget.Remove(h);
            }

        }
    }
    private void OnDestroy()
    {
        enemiesOnTarget.Clear();
    }
}
