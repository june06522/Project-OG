using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Material _shockwaveMaterial;
    private Coroutine _coroutine = null;

    public bool IsPlay => (_coroutine != null);

    private readonly int _waveDistanceFromCenterHash = Shader.PropertyToID("_WaveDistanceFromCenter");
    private readonly int _strengthHash = Shader.PropertyToID("_Strength");

    private void Awake()
    {
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
        _shockwaveMaterial = _spriteRenderer?.material;

        
        _shockwaveMaterial = _spriteRenderer.material = Instantiate(_shockwaveMaterial);
    }

    public void PlayShockwave(float strength, float endValue, float time)
    {
        if (_shockwaveMaterial == null)
            return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ShockwaveCoroutine(strength, endValue, time));
    }

    IEnumerator ShockwaveCoroutine(float strength, float endValue, float time)
    {
        _spriteRenderer.enabled = true;

        _shockwaveMaterial.SetFloat(_waveDistanceFromCenterHash, 0f);
        _shockwaveMaterial.SetFloat(_strengthHash, strength);

        float currentTime = 0f;
        float lerpValue = 0f;

        if (endValue > 1f)
            endValue = 1f;

        while (currentTime <= time)
        {
            lerpValue = Mathf.Lerp(0, endValue, currentTime / time);
            _shockwaveMaterial.SetFloat(_waveDistanceFromCenterHash, lerpValue);

            currentTime += Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.enabled = false;
        _coroutine = null;
    }

}
