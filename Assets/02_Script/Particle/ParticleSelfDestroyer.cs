using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleSelfDestroyer : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float range;

    List<Collider2D> cols = new List<Collider2D>();

    public void Attack(float damage)
    {

        cols = Physics2D.OverlapCircleAll(transform.position, range, targetLayer).ToList();

        foreach (var col in cols)
        {

            if (col.TryGetComponent<IHitAble>(out var item))
            {

                item.Hit(damage);

            }

        }

    }

    public void EndOfAnimation()
    {

        Destroy(gameObject, 0.05f);

    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
#endif

}
