using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoToDash : FSM_Transition<EnumPlayerState>
{
    private PlayerEnerge _playerEnerge;

    private PlayerDataSO data;
    private PlayerInputController inputController => PlayerController.InputController;

    public PlayerGoToDash(PlayerController controller, PlayerEnerge playerEnerge) : base(controller, EnumPlayerState.Dash)
    {

        data = controller.playerData;
        _playerEnerge = playerEnerge;
    }

    protected override bool CheckTransition()
    {

        if(inputController == null)
        {

            return false;

        }

        return inputController.isDashKeyPressed && !data[PlayerCoolDownType.Dash] && _playerEnerge.ConsumeEnerge(10);

    }


}
