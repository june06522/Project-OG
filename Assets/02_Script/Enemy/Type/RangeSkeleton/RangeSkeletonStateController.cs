using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSkeletonStateController : BaseFSM_Controller<ENormalEnemyState>
{
    public Transform target;

    [SerializeField]
    private EnemyBullet _bullet;
    [SerializeField]
    private Transform _weapon;

    protected override void Start()
    {
        base.Start();

        var rootState = new NormalRootState(this);
        rootState
            .AddTransition<ENormalEnemyState>(new RoomOpenTransitions<ENormalEnemyState>(this, ENormalEnemyState.Move));

        var moveState = new NormalChaseState(this);
        var moveToAttack = new MoveToAttackTransition<ENormalEnemyState>(this, ENormalEnemyState.Attack, true);

        moveState
            .AddTransition<ENormalEnemyState>(moveToAttack);

        // ½ºÄÌ·¹Åæ ¾îÅÃ
        var attackState = new BulletAttackState(this, _bullet, _weapon);

        AddState(rootState, ENormalEnemyState.Idle);
        AddState(moveState, ENormalEnemyState.Move);
        AddState(attackState, ENormalEnemyState.Attack);
    }
}
