using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDamageObject : MonoBehaviour
{
    [SerializeField]
    private float _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.TryGetComponent<IHitAble>(out IHitAble hit))
        {
            hit.Hit(_damage);
        }
    }
}
