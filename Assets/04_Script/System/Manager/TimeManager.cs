using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static TimeManager instance;

    private bool isStoped;

    private void Awake()
    {
        
        instance = this;

    }

    public void Stop(float time, float duration)
    {

        if (isStoped) return;

        StartCoroutine(StopCo(time, duration));

    }

    private IEnumerator StopCo(float time, float duration)
    {

        isStoped = true;

        Time.timeScale = time;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;

        isStoped = false;

    }

}
