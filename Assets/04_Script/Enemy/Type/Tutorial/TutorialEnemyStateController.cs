using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyStateController : BaseFSM_Controller<ENormalPatrolEnemyState>
{
    [NonSerialized]
    public Transform attackPoint;

    [SerializeField] private TutorialEnemyBullet _bullet;

    [SerializeField]
    private AudioClip _lazerClip;

    TutorialEnemyAttackState state;

    protected override void Awake()
    {
        base.Awake();
        attackPoint = transform.Find("AttackPoint").GetComponent<Transform>();

    }

    protected override void Start()
    {
        base.Start();

        var rootState = new NormalPatrolRootState(this);
        var rootToPatrol = new RoomOpenTransitions<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Patrol);
        rootState
            .AddTransition<ENormalPatrolEnemyState>(rootToPatrol);

        var patrolState = new NormalPatrolState(this);
        var patrolToMove = new PatrolToChaseTransition<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Move);
        patrolState
             .AddTransition<ENormalPatrolEnemyState>(patrolToMove);

        var moveState = new NormalPatrolChaseStae(this);
        var moveToAttack = new MoveToAttackTransition<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Attack, true);
        moveState
            .AddTransition<ENormalPatrolEnemyState>(moveToAttack);

        var attackState = new TutorialEnemyAttackState(this);
        state = attackState;
        AddState(rootState, ENormalPatrolEnemyState.Idle);
        AddState(patrolState, ENormalPatrolEnemyState.Patrol);
        AddState(moveState, ENormalPatrolEnemyState.Move);
        AddState(moveState, ENormalPatrolEnemyState.Attack);
    }

    public void Shoot()
    {
        SoundManager.Instance.SFXPlay("Lazer", _lazerClip);
        Instantiate(_bullet,transform.position,Quaternion.identity);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Attack()
    {
        state.AttackState();
    }

    public void AttackLoop()
    {
        StartCoroutine(AttackLoopCo());
    }

    IEnumerator AttackLoopCo()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.2f);
            Attack();
        }
    }
}
