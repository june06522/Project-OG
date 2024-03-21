using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    private float _realTime = 0;

    private Boss _thisBoss;

    [SerializeField]
    private Image _backGround;
    [SerializeField]
    private Image _showBox;

    private void Awake()
    {
        _thisBoss = GetComponent<Boss>();
        _backGround.gameObject.SetActive(false);
        _showBox.gameObject.SetActive(false);
    }

    private void Start()
    {
        //StartCoroutine(CutSceneOn());
    }

    void Update()
    {
        if(_thisBoss.awakening)
        {
            StartCoroutine(RealTimer());
            StartCoroutine(CutSceneOn(3, 1, 0.5f, 400, 50));
            _thisBoss.awakening = false;
        }
    }

    private IEnumerator RealTimer()
    {
        Time.timeScale = 0;
        float beforeTime = 0;
        float laterTime = 0;
        while(Time.timeScale == 0)
        {
            beforeTime = Time.realtimeSinceStartup;

            yield return null;

            laterTime = Time.realtimeSinceStartup;
            _realTime = laterTime - beforeTime;
        }
    }

    private IEnumerator CutSceneOn(float end, float firstWait, float waitTime, float turnSpeed, float shutDownSpeed)
    {
        yield return new WaitForSecondsRealtime(firstWait);

        _backGround.gameObject.SetActive(true);
        _showBox.gameObject.SetActive(true);
        _showBox.transform.localScale = new Vector3(30, 0, 1);

        while (_showBox.transform.localScale.y <= end / 2)
        {
            _showBox.transform.localScale += new Vector3(0, _realTime, 0);
            yield return null;
        }

        float curTime = 0;
        while(curTime < waitTime)
        {
            curTime += _realTime;
            _showBox.transform.Rotate(0, 0, _realTime * turnSpeed);
            yield return null;
        }

        curTime = 0;
        while(_showBox.transform.localScale.y < end)
        {
            _showBox.transform.localScale += new Vector3(0, _realTime, 0);
            yield return null;
        }

        while(_showBox.transform.localScale.y > 0)
        {
            _showBox.transform.localScale -= new Vector3(0, _realTime * shutDownSpeed);
            yield return null;
        }

        _backGround.gameObject.SetActive(false);
        _showBox.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
