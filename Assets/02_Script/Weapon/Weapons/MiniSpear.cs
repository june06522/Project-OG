using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSpear : MonoBehaviour
{

    private Transform target;
    private Vector2 origin;

    private void Update()
    {
        
        if(target == null)
        {

            FAED.InsertPool(gameObject);
            return;

        }

        var dir = target.position - transform.position;
        transform.up = dir.normalized;

    }

    public void SetUp(Transform target)
    {

        origin = transform.position;
        this.target = target;
        StartCoroutine(FollowTargetCo());

    }

    private IEnumerator FollowTargetCo()
    {

        float per = 0;

        while (per < 1)
        {

            transform.position = Vector3.Lerp(origin, target.position, per);
            per += Time.deltaTime;
            yield return null;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("HitAble"))
        {

            if (collision.TryGetComponent<IHitAble>(out var hit))
            {

                hit.Hit(Random.Range(5, 15));
                FAED.InsertPool(gameObject);

            }

        }

    }

}
