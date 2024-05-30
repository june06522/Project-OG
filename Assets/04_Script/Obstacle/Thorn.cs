using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Thorn : MonoBehaviour
{
    [SerializeField] AnimationCurve appearEase;
    [SerializeField] AnimationCurve disappearEase;

    [SerializeField] float appearDuration;
    [SerializeField] float disappearDuration;

    [SerializeField] float moveValue = 100f;

    Vector2 originPos;
    Vector2 targetPos;

    bool isAppear;

    private void Awake()
    {
        originPos = transform.position;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Appear();
        }
    }

    private void Appear()
    {
        isAppear = !isAppear;

        Vector2 movePoint = isAppear
            ? originPos + (Vector2)transform.up * moveValue
            : originPos;

        float duration = isAppear ? appearDuration : disappearDuration;
        AnimationCurve ease = isAppear ? appearEase : disappearEase;

        transform.DOMove(movePoint, duration).SetEase(ease);
    }
}
