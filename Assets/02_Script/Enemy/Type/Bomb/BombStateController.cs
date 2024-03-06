using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBombState
{
    Idle = 0,
    Move = 1,
    Attack,
}

public class BombStateController : BaseFSM_Controller<EBombState>
{
    public ParticleSystem bomb;

    protected override void Start()
    {
        base.Start();

        var rootState = new BombRootState(this);
        var rootToChase = new TransitionIdleOrMove<EBombState>(this, EBombState.Move);
        rootState.AddTransition(rootToChase);

        var chaseState = new BombChaseState(this);
        var chaseToAttack = new MoveToAttackTransition<EBombState>(this, EBombState.Attack, true);
        chaseState
            .AddTransition<EBombState>(chaseToAttack);

        var attackState = new BombAttackState(this);

        AddState(rootState, EBombState.Idle);
        AddState(chaseState, EBombState.Move);
        AddState(attackState, EBombState.Attack);
    }

    public void Boom()
    {
        //Instantiate(bomb, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
