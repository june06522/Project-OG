using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETestEnemyState
{
    Idle = 0,
    Move = 1,
    Jump = 2,
    Dash = 3,
}

public class TestEnemyFSMController : BaseFSM_Controller<ETestEnemyState>, IHitAble
{
    [SerializeField] public GameObject grid;
    [SerializeField] public EnemyBullet bullet;

    public FeedbackPlayer feedbackPlayer { get; set; }

    protected override void Awake()
    {
        feedbackPlayer = GetComponent<FeedbackPlayer>();    
        base.Awake();
        var idleState = new TestEnemyRootState(this);
        var idleToMove = new TransitionIdleOrMove<ETestEnemyState>(this, ETestEnemyState.Move);
        
        //idleState
        //    .AddTransition<ETestEnemyState>(idleToMove);
        
        var moveState = new TestEnemyMoveState(this);
        var moveToIdle = new TransitionIdleOrMove<ETestEnemyState>(this, ETestEnemyState.Idle);
        var goToDash = new TestEnemyDashTransition(this, ETestEnemyState.Dash);
        var goToJump = new TestEnemyJumpTransition(this, ETestEnemyState.Jump);

        moveState
            .AddTransition<ETestEnemyState>(moveToIdle)
            .AddTransition<ETestEnemyState>(goToDash)
            .AddTransition<ETestEnemyState>(goToJump);

        var dashState = new TestEnemyDashState(this);
        var jumpState = new TestEnemyJumpState(this);
        
        AddState(idleState, ETestEnemyState.Idle);
        AddState(moveState, ETestEnemyState.Move);
        AddState(dashState, ETestEnemyState.Dash);
        AddState(jumpState, ETestEnemyState.Jump);
    }

    public void IgnoreCollision(bool value)
    {
        Physics2D.IgnoreCollision(Enemy.Collider, GameManager.Instance.player.GetComponent<Collider2D>());
    }

    //Debug
    public void InstantiateDebugGrid(Vector2 pos)
    {
        Destroy(Instantiate(grid, pos, Quaternion.identity), 2);
    }

    public void InstantiateBullet(Vector2 dir, EEnemyBulletSpeedType speedType, EEnemyBulletCurveType curveType = EEnemyBulletCurveType.None)
    {
        Instantiate(bullet, transform.position, Quaternion.identity).Shoot(dir, speedType, curveType);
    }
}
