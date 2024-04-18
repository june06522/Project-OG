using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDamageObject : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField]
    private float _touchDamage;
    [SerializeField]
    private float _overlapedDamage;

    [Header("Overlaped Check Time")]
    [SerializeField]
    private float _overlapedTime = 1f;
    private float _currentTime = 0f;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.TryGetComponent<IHitAble>(out IHitAble hit))
        {
            _currentTime = 0f;

            hit.Hit(_touchDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _currentTime += Time.deltaTime;

            if (_overlapedTime <= _currentTime)
            {
                _currentTime -= _overlapedTime;

                if(collision.TryGetComponent<IHitAble>(out IHitAble hit))
                {
                    hit.Hit(_overlapedDamage);
                }
            }
        }
    }
}
