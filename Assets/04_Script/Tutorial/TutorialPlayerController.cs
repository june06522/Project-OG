using DG.Tweening;
using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class TutorialPlayerController : PlayerController
{
    [HideInInspector]
    public bool canMove = false;

    [HideInInspector]
    public Rigidbody2D rb2d;

    ParticleSystem moveParticle;
    GameObject _visual;

    protected override void Awake()
    {
        base.Awake();
        rb2d = GetComponent<Rigidbody2D>();

        moveParticle = GameObject.Find("Player/PlayerVisual/MoveParticle").GetComponent<ParticleSystem>();
        _visual = GameObject.Find("Player/PlayerVisual");
    }

    protected override void Update()
    {
        if(canMove)
            base.Update();

    }


    public void Stop()
    {
        currentState = EnumPlayerState.Move;
        rb2d.velocity = Vector3.zero;
        moveParticle.Stop();
        DOTween.Sequence()
            .Append(_visual.transform.DOScale(new Vector2(1.3f, 0.8f), 0.2f).SetEase(Ease.InOutBack))
            .Append(_visual.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce));

    }

    public void SetRotation(Quaternion dir)
    {
        _visual.transform.rotation = dir;
    }

    public void SetEnergy(int energy = 20)
    {
        _playerEnerge.TutoMinusEnergy(energy);
    }
}
