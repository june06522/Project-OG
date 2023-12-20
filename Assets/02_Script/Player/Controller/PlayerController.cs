using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumPlayerState
{

    Idle,
    Move,
    Dash

}


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : FSM_Controller<EnumPlayerState>
{

    [field: SerializeField] public PlayerDataSO playerData { get; protected set; }
    [SerializeField] private GameObject inv;

    private static PlayerInputController inputController;
    private static PlayerEventController eventController;

    public static PlayerInputController InputController => inputController;
    public static PlayerEventController EventController => eventController;

    protected override void Awake()
    {

        inputController = new PlayerInputController();
        eventController = new PlayerEventController();

        playerData = Instantiate(playerData);
        playerData.SetOwner(this);

        var goToDash = new PlayerGoToDash(this);

        var idleState = new PlayerRootState(this);
        var idleToMove = new PlayerTransitionByMoveDir(this, EnumPlayerState.Move, Vector2.zero, TransitionCheckType.Greater);

        idleState
            .AddTransition<EnumPlayerState>(idleToMove)
            .AddTransition<EnumPlayerState>(goToDash);

        var moveState = new PlayerMoveState(this);
        var moveToIdle = new PlayerTransitionByMoveDir(this, EnumPlayerState.Idle, Vector2.zero, TransitionCheckType.Equal);

        moveState
            .AddTransition<EnumPlayerState>(moveToIdle)
            .AddTransition<EnumPlayerState>(goToDash);

        var dashState = new PlayerDashState(this);

        AddState(idleState, EnumPlayerState.Idle);
        AddState(moveState, EnumPlayerState.Move);
        AddState(dashState, EnumPlayerState.Dash);



    }

    protected override void Update()
    {

        base.Update();

        inputController.Update();

        if (Input.GetKeyDown(KeyCode.V))
        {

            inv.SetActive(!inv.activeSelf);

        }

    }

    private void OnDestroy()
    {

        inputController.Dispose();
        eventController.Dispose();

        inputController = null;
        eventController = null;

    }

}
