using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;

public class PlayerRootState : FSM_State<EnumPlayerState>
{

    protected Rigidbody2D rigid;
    protected PlayerInputController inputController => PlayerController.InputController;

    public PlayerRootState(FSM_Controller<EnumPlayerState> controller) : base(controller)
    {

        rigid = GetComponent<Rigidbody2D>();

    }

}
