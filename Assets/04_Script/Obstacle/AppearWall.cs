using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearWall : MonoBehaviour
{
    [SerializeField]
    Color highLightColor = Color.white;

    float appearDuration = 0.5f;
    float disappearDuration = 0.5f;
    float delay = 0.5f;

    Sequence seq;
    SpriteRenderer _renderer;
    Collider2D col;

    Vector2 _originScale;
    Color _originColor;

    public event Action WallAppearEndEvent;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _originScale = transform.localScale;
        _originColor = _renderer.color;
        Init();
    }

    public void Init()
    {
        _renderer.color = new Color(_originColor.r, _originColor.g, _originColor.b, 0f);
        transform.localScale = new Vector3(0f, _originScale.y, 1f);
        col.enabled = false;
    }

    public void Prepare()
    {
        transform.localScale = Vector2.zero;
        _renderer.color = highLightColor;
    }

    private void Update()
    {
        //if (
        //(KeyCode.Space))
        //    Appear();
    }

    public void Appear()
    {
        seq.Kill();
        seq = DOTween.Sequence();

        seq.Append(transform.DOScaleX(_originScale.x, appearDuration));
        seq.Join(_renderer.DOFade(0.6f, appearDuration)).AppendCallback(() => Prepare());
        
        seq.AppendInterval(0.1f);
        seq.Append(transform.DOScale(_originScale, 0.125f));
        
        seq.AppendCallback(() => CanCollisionEnter());
        seq.AppendInterval(delay);
        seq.AppendCallback(() => CanCollisionExit());

        seq.Append(transform.DOScaleX(0f, disappearDuration));
        seq.Join(_renderer.DOFade(0f, disappearDuration));

        seq.OnComplete(() => EndDisAppear());
    }

    private void EndDisAppear()
    {
        Init();
        WallAppearEndEvent?.Invoke();
    }

    private void CanCollisionEnter()
    {
        CameraManager.Instance.CameraShake(1.5f, 0.25f);
        _renderer.color = _originColor;
        col.enabled = true;
    }

    private void CanCollisionExit()
    {
        col.enabled = false;
    }
}
