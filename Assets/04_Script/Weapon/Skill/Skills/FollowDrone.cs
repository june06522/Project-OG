using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDrone : MonoBehaviour
{
    [SerializeField] Bullet bullet;
    private float lifeTime;
    bool isAlive = false;

    Transform target;

    public void SetLifeTime(float lifeTime, Transform target)
    {

        this.target = target;
        this.lifeTime = lifeTime;
        gameObject.transform.parent = this.target;
        
        isAlive = true;

        var dir = target.position - transform.position;
        dir.Normalize();
        dir.z = 0;
        transform.right = dir;

        StartCoroutine(AttackCo());
    }

    private void Update()
    {

        if (isAlive == true)
        {

            lifeTime -= Time.deltaTime;
            if (lifeTime < 0)
            {

                isAlive = false;

                // 후에 풀링으로 변경
                Destroy(gameObject);

            }

        }

    }

    IEnumerator AttackCo()
    {
        YieldInstruction wait = new WaitForSeconds(1f);
        
        while (gameObject.activeSelf)
        {
            yield return wait;

            var blt = Instantiate(bullet, transform.position, transform.rotation);
            blt.Shoot(bullet.Data.Damage);

            transform.DOShakePosition(0.1f, 0.25f);

        }

    }

}
