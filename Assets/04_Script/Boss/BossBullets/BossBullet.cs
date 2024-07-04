using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] protected BossBulletDataSO data;

    [SerializeField] protected ParticleSystem particle;

    [SerializeField] protected bool isRotateBullet;

    [SerializeField] private Color _color;

    [SerializeField] private bool _isNotBlocked;

    private float f_currentDamage = 0;

    protected virtual void OnEnable()
    {
        if(particle != null)
        {
            particle.Play();
        }
        if(data.DestoryTime != 0)
        {
            StartCoroutine(ObjectPool.Instance.ReturnObject(this.gameObject, data.DestoryTime));
        }
    }

    protected virtual void OnDisable()
    {
        if(_color != new Color(0, 0, 0, 0))
        {
            transform.GetComponent<SpriteRenderer>().color = _color;
        }
        
        StopAllCoroutines();
    }

    public void Attack(float bossDamage)
    {
        f_currentDamage = bossDamage + data.Damage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHP player;

        if (collision.gameObject.TryGetComponent<PlayerHP>(out player))
        {
            player.Hit(data.Damage);
            if(data.IfHitWillBreak)
            {
                ObjectPool.Instance.ReturnObject(this.gameObject);
            }
        }

        if(!_isNotBlocked)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                ObjectPool.Instance.ReturnObject(this.gameObject);
            }
        }
    }

}
