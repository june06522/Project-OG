using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveEffect : MonoBehaviour
{
    private readonly int _hashWaveDistance = Shader.PropertyToID("_WaveDistance");
    
    private Tween _shockTween;
    private Material _material;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
        _spriteRenderer.gameObject.SetActive(false);
    }

    public void ShockwaveEffect()
    {
        //if (_shockTween != null && _shockTween.IsActive())
        //{
        //    _shockTween.Kill();
        //}

        //_spriteRenderer.gameObject.SetActive(true);
        //_material.SetFloat(_hashWaveDistance, -0.1f);

        //_shockTween = DOTween.To(
        //    () => _material.GetFloat(_hashWaveDistance),
        //    value => _material.SetFloat(_hashWaveDistance, value),
        //    1f, 0.6f)
        //    .OnComplete(() => _spriteRenderer.gameObject.SetActive(false));

    }
}
