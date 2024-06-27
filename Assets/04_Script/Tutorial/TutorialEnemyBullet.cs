using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyBullet : EnemyBullet
{
    private void Awake()
    {
        isDestroy = false;
        Shoot(GameManager.Instance.player.position - transform.position);
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (_dataSO == null) return;
        if (collision.CompareTag(_dataSO.HitAbleTag[0]))
        {
            IHitAble hitAble;
            if (collision.TryGetComponent<IHitAble>(out hitAble))
            {
                hitAble.Hit(_dataSO.Damage);
                Destroy(gameObject);
            }
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float val) => curSpeed = val;
}