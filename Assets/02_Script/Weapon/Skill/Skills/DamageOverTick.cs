using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTick : MonoBehaviour
{
    float startTime;

    IHitAble hp;

    public void Init(float damage, float time)
    {

        startTime = Time.time;
        StartCoroutine(Tick(damage, time));

    }

    IEnumerator Tick(float damage, float time)
    {

        hp = GetComponent<IHitAble>();

        for (int i = 0; i < time / 0.5f; i++)
        {

            hp.Hit(damage);
            yield return new WaitForSeconds(0.5f);

        }

        Destroy(this);

    }
}
