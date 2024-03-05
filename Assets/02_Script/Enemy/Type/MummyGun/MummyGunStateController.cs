using FSM_System;
using UnityEngine;

public enum EMummyGunState
{
    Idle = 0,
    Patrol = 1,
    Move = 2,
    Attack,
}

public class MummyGunStateController : BaseFSM_Controller<EMummyGunState>
{
    public Transform DebugTile;
    [SerializeField]
    public Transform attackPoint;
    [SerializeField]
    public EnemyBullet bullet;


    protected override void Start()
    {
        base.Start();

        var rootState = new MummyGunRootState(this);
        var rootToPatrol = new RoomOpenTransitions<EMummyGunState>(this, EMummyGunState.Patrol);
        rootState.
            AddTransition<EMummyGunState>(rootToPatrol);
        
        var patrolState = new MummyGunPatrolState(this);
        var patrolToMove = new PatrolToChaseTransition<EMummyGunState>(this, EMummyGunState.Move);
        patrolState
             .AddTransition<EMummyGunState>(patrolToMove);

        var moveState = new MummyGunMoveState(this);
        var moveToAttack = new MoveToAttackTransition<EMummyGunState>(this, EMummyGunState.Attack, true);
        moveState
            .AddTransition<EMummyGunState>(moveToAttack);

        var attackState = new MummyGunAttackState(this);

        AddState(rootState, EMummyGunState.Idle); 
        AddState(patrolState, EMummyGunState.Patrol);
        AddState(moveState, EMummyGunState.Move);
        AddState(attackState, EMummyGunState.Attack);
    }

    public void InstantiateBullet(Vector2 dir, EEnemyBulletSpeedType speedType, EEnemyBulletCurveType curveType = EEnemyBulletCurveType.None)
    {
        Instantiate(bullet, attackPoint.position, Quaternion.identity).Shoot(dir, speedType, curveType);
    }
}
