using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMoveState : PlayerRootState
{
    PlayerController playerController;
    bool increase = false;

    public PlayerMoveState(PlayerController controller) : base(controller)
    {
        playerController = controller;
    }

    protected override void EnterState()
    {
        playerController.moveParticle.Play();
        transform.DOScale(new Vector2(0.8f, 1.3f), 0.2f).SetEase(Ease.InOutBack);
    }

    protected override void UpdateState()
    {

        //rigid.transform.up = inputController.MoveDir;

        float rotateDegree = Mathf.Atan2(inputController.MoveDir.y, inputController.MoveDir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, rotateDegree - 90);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 15);

        if (increase)
        {
            // upscale
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + Time.deltaTime * 3, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - Time.deltaTime * 3, transform.localScale.z);
        }

        if (transform.localScale.y < 1.2f)
        {
            increase = true;
        }
        if (transform.localScale.y > 1.4f)
        {
            increase = false;
        }

        rigid.velocity = transform.up * playerData.MoveSpeed;
    }


    protected override void ExitState()
    {

        playerController.moveParticle.Stop();
        DOTween.Sequence()
            .Append(transform.DOScale(new Vector2(1.3f, 0.8f), 0.2f).SetEase(Ease.InOutBack))
            //.Append(transform.DOScale(Vector3.one * 1.4f, 0.2f).SetEase(Ease.OutBounce))
            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce));

        rigid.velocity = Vector2.zero;

    }

}