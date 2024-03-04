using FSM_System;
using UnityEngine;

public enum EMummyGunState
{
    Idle = 0,
    Move = 1,
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

        var patrolState = new MummyGunRootState(this);
        var patrolToMove = new MummyGunPatrolToMoveTransition(this, EMummyGunState.Move);

        //patrolState
        //     .AddTransition<EMummyGunState>(patrolToMove);

        var moveState = new MummyGunMoveState(this);
        var moveToAttack = new MoveToAttackTransition<EMummyGunState>(this, EMummyGunState.Attack, true);

        moveState
            .AddTransition<EMummyGunState>(moveToAttack);

        var attackState = new MummyGunAttackState(this);

        AddState(patrolState, EMummyGunState.Idle);
        AddState(moveState, EMummyGunState.Move);
        AddState(attackState, EMummyGunState.Attack);

    }

    public void InstantiateBullet(Vector2 dir, EEnemyBulletSpeedType speedType, EEnemyBulletCurveType curveType = EEnemyBulletCurveType.None)
    {
        Instantiate(bullet, attackPoint.position, Quaternion.identity).Shoot(dir, speedType, curveType);
    }
}
