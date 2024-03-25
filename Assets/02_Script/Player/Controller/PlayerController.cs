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
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private GameObject _interactKey;

    [field: SerializeField] public PlayerDataSO playerData { get; protected set; }

    private static PlayerInputController inputController;
    private static PlayerEventController eventController;

    public static PlayerInputController InputController => inputController;
    public static PlayerEventController EventController => eventController;

    private readonly int idleHash = Animator.StringToHash("IsIdle");

    protected override void Awake()
    {
        _interactKey = GameManager.Instance.transform.Find("InteractKey")?.gameObject;

        inputController = new PlayerInputController();
        eventController = new PlayerEventController();


        if(_interactKey != null)
            inputController.SetInteractUI(_interactKey);

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

        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        InputController.Particle = GetComponent<ParticleSystem>();

    }

    protected override void Update()
    {

        base.Update();

        inputController.Update();

        _animator?.SetBool(idleHash, InputController.MoveDir == Vector2.zero);
        if (InputController.LastMoveDir.x != 0)
            _spriteRenderer.flipX = InputController.LastMoveDir.x > 0;

    }

    private void OnDestroy()
    {

        inputController.Dispose();
        eventController.Dispose();

        inputController = null;
        eventController = null;

    }

}
