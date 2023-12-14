using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerRootState
{

    public PlayerMoveState(PlayerController controller) : base(controller)
    {
    }

    protected override void UpdateState()
    {

        rigid.velocity = inputController.MoveDir * playerData.MoveSpeed;

    }


    protected override void ExitState()
    {

        rigid.velocity = Vector2.zero;

    }

}