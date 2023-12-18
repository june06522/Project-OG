using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionCheckType
{

    Equal,
    //MoveDir이 크다
    Greater,
    //MoveDir이 작다
    Less

}

public class PlayerTransitionByMoveDir : FSM_Transition<EnumPlayerState>
{

    private TransitionCheckType checkType;
    private Vector2 checkValue;

    public PlayerTransitionByMoveDir(FSM_Controller<EnumPlayerState> controller, EnumPlayerState nextState, Vector2 value, TransitionCheckType checkType) : base(controller, nextState)
    {

        checkValue = value;
        this.checkType = checkType;

    }

    protected override bool CheckTransition()
    {

        if(PlayerController.InputController == null) return false;

        switch (checkType)
        {

            case TransitionCheckType.Equal:
                return PlayerController.InputController.MoveDir == checkValue;

            case TransitionCheckType.Greater:
                return PlayerController.InputController.MoveDir.sqrMagnitude > checkValue.sqrMagnitude;

            case TransitionCheckType.Less:
                return PlayerController.InputController.MoveDir.sqrMagnitude < checkValue.sqrMagnitude;

            default:
                return false;

        }

    }

}
