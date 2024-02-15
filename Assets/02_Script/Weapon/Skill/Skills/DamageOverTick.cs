using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTick : MonoBehaviour
{
    float startTime;

    public void Init(float damage, float time)
    {
        startTime = Time.time;
        StartCoroutine(Tick(damage, time));
    }

    IEnumerator Tick(float damage, float time)
    {
        for (int i = 0; i < time / 0.5f; i++)
        {
            yield return new WaitForSeconds(0.5f);

        }

        Destroy(this);
    }
}
