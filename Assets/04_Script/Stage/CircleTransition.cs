using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransition : MonoBehaviour
{
    private Vector3 _resolution = new Vector3(1920, 1080);
    public float CircleMaxValue => ((_resolution.y > _resolution.x) ? _resolution.y : _resolution.x) + 300;

    private Image _image;
    private Transform _player;
    private Material _circleMat;

    private const string resolutionVecName = "_Resolution";
    private const string circlePosVecName = "_CirclePos";
    private const string circleSizeVecName = "_CircleSize";

    private Coroutine _coroutine;

    private void Start()
    {
        _player = GameManager.Instance.player;
        _image = GetComponent<Image>();
        _circleMat = _image.material = Instantiate(_image.material);

        SetResolution();
    }

    public void PlayCircleSizeChange(Vector3 startSize, Vector3 endSize, float time, bool lastTween = false)
    {
        SetOnOff(true);

        // Resolution Setting 
        SetResolution();

        Vector3 pos = Camera.main.WorldToScreenPoint(_player.position) - (_resolution * 0.5f);
        _circleMat.SetVector(circlePosVecName, pos);

        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(CircleSizeChange(startSize, endSize, time, lastTween));
    }

    private void SetResolution()
    {
        Resolution resolution = Screen.currentResolution;
        Vector3 resolutionVec = new Vector3(resolution.width, resolution.height, 0);

#if DEBUG
        resolutionVec = new Vector3(1920, 1080);
#endif

        if (_resolution != resolutionVec)
        {
            _circleMat.SetVector(resolutionVecName, resolutionVec);
            Screen.SetResolution((int)_resolution.x, (int)_resolution.y, true);
        }
    }

    public void SetOnOff(bool onOff)
    {
        _image.enabled = onOff;
    }

    IEnumerator CircleSizeChange(Vector3 startSize, Vector3 endSize, float time, bool lastTween = false)
    {
        float currentTime = 0;
        Vector3 circleS = Vector3.one * 300;

        if (lastTween)
            endSize += circleS;

        while (currentTime < time)
        {
            _circleMat.SetVector(circleSizeVecName,
                Vector3.Lerp(startSize, endSize, currentTime / time));

            yield return null;
            currentTime += Time.deltaTime;
        }
        _circleMat.SetVector(circleSizeVecName, endSize);

        if(lastTween)
        {
            float delayTime = 0.8f;
            yield return new WaitForSeconds(delayTime);

            currentTime = 0;
            float animationTime1 = 0.1f;
            while (currentTime < animationTime1)
            {
                _circleMat.SetVector(circleSizeVecName,
                    Vector3.Lerp(endSize,
                    endSize + Vector3.one * 20,
                    currentTime / animationTime1));

                yield return null;
                currentTime += Time.deltaTime;
            }
            currentTime = 0;
            float animationTime2 = 0.1f;
            while (currentTime < animationTime2)
            {
                _circleMat.SetVector(circleSizeVecName,
                    Vector3.Lerp(endSize + Vector3.one * 20,
                    Vector3.zero,
                    currentTime / animationTime2));

                yield return null;
                currentTime += Time.deltaTime;
            }
            _circleMat.SetVector(circleSizeVecName, Vector3.zero);

        }

        _coroutine = null;
    }
}
