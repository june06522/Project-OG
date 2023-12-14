using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumPlayerState
{

    Idle,
    Move,

}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : FSM_Controller<EnumPlayerState>
{

    [field: SerializeField] public PlayerDataSO playerData { get; protected set; }

    private static PlayerInputController inputController;

    public static PlayerInputController InputController => inputController;

    protected override void Awake()
    {

        inputController = new PlayerInputController();

        var idleState = new PlayerRootState(this);
        var idleToMove = new PlayerTransitionByMoveDir(this, EnumPlayerState.Move, Vector2.zero, TransitionCheckType.Greater);

        idleState.AddTransition(idleToMove);

        var moveState = new PlayerMoveState(this);
        var moveToIdle = new PlayerTransitionByMoveDir(this, EnumPlayerState.Idle, Vector2.zero, TransitionCheckType.Equal);

        moveState.AddTransition(moveToIdle);

        AddState(idleState, EnumPlayerState.Idle);
        AddState(moveState, EnumPlayerState.Move);

    }

    protected override void Update()
    {

        base.Update();

        inputController.Update();

    }

    private void OnDestroy()
    {

        inputController.Dispose();
        inputController = null;

    }

}
