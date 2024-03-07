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
        Collider2D collider = Physics2D.OverlapCircle(instShowRangeObj.transform.position, radius, LayerMask.GetMask("Player"));
        IHitAble hitAble;
        if(collider != null)
        {
            if(collider.TryGetComponent<IHitAble>(out hitAble))
            {
                Debug.Log("Hit");
                hitAble.Hit(EnemyDataSO.AttackPower);
            }
            //collider.gameObject.GetComponent<Rigidbody2D>().
            //    AddForce((collider.transform.position- instShowRangeObj.transform.position).normalized * 100, ForceMode2D.Impulse) ;
        }

        if(instShowRangeObj != null)
            Destroy(instShowRangeObj);
        Destroy(this.gameObject);
    }

    public void InstantiateRange()
    {
        instShowRangeObj = Instantiate(showRange, transform.position, Quaternion.identity);
    }
}
