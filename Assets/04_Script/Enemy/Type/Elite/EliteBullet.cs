using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EliteBullet : MonoBehaviour
{
    [SerializeField]
    protected float destroyTime;
    [SerializeField]
    protected float damage;

    protected virtual void OnEnable()
    {
        Destroy(gameObject, destroyTime);
        StartCoroutine(InfinityRotateZ(gameObject, 1000));
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected IEnumerator InfinityRotateZ(GameObject obj, float speed = 1)
    {
        float angle = 0;
        while(true)
        {
            angle += Time.deltaTime * speed;

            if(angle < 360)
            {
                obj.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                angle = 0;
            }

            yield return null;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHP player;
        if (collision.transform.TryGetComponent<PlayerHP>(out player))
        {
            player.Hit(damage);
            UnityEngine.Object.Destroy(gameObject);
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
