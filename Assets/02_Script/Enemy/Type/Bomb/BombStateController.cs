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
    public GameObject showRange;
    public float radius = 3f;
    private GameObject instShowRangeObj;

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
        Collider2D collider = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));
        IHitAble hitAble;
        if(collider.TryGetComponent<IHitAble>(out hitAble))
        {
            Debug.Log("Hit");
            hitAble.Hit(EnemyDataSO.AttackPower);
        }

        Destroy(this.gameObject);
        if(instShowRangeObj != null)
            Destroy(instShowRangeObj);
    }

    public void InstantiateRange()
    {
        instShowRangeObj = Instantiate(showRange, transform.position, Quaternion.identity);
    }
}
