using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;

public class PlayerRootState : FSM_State<EnumPlayerState>
{

    protected PlayerDataSO playerData;
    protected Rigidbody2D rigid;
    protected PlayerInputController inputController => PlayerController.InputController;
    protected PlayerEventController eventController => PlayerController.EventController;

    public PlayerRootState(PlayerController controller) : base(controller)
    {

        playerData = controller.playerData;
        rigid = GetComponent<Rigidbody2D>();

    }

}
