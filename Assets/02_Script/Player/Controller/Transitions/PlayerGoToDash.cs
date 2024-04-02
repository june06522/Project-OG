using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoToDash : FSM_Transition<EnumPlayerState>
{
    private PlayerEnerge _playerEnerge;

    private PlayerDataSO data;
    private PlayerInputController inputController => PlayerController.InputController;
    private AudioClip clip;

    public PlayerGoToDash(PlayerController controller, PlayerEnerge playerEnerge, AudioClip clip) : base(controller, EnumPlayerState.Dash)
    {

        data = controller.playerData;
        _playerEnerge = playerEnerge;
        this.clip = clip;
    }

    protected override bool CheckTransition()
    {

        if(inputController == null)
        {

            return false;

        }

        bool returnVal = inputController.isDashKeyPressed && !data[PlayerCoolDownType.Dash] && _playerEnerge.ConsumeEnerge(10);

        if(returnVal)
            SoundManager.Instance?.SFXPlay("Dash", clip);

        return returnVal;

    }


}
