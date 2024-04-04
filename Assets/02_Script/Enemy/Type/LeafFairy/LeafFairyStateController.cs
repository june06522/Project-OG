using FSM_System;
using System;
using System.Collections;
using UnityEngine;


public class LeafFairyStateController : BaseFSM_Controller<ENormalPatrolEnemyState>
{
    [SerializeField]
    public ParticleSystem attackEffect;
    [SerializeField]
    public Transform attackPoint;
    [SerializeField]
    public LeafBullet bullet;

    [SerializeField]
    private AudioClip _attackSound;

    GameObject particleOBJ;
    protected override void Start()
    {
        base.Start();

        var rootState = new NormalPatrolRootState(this);
        var rootToMove = new RoomOpenTransitions<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Patrol); 
        rootState
            .AddTransition<ENormalPatrolEnemyState>(rootToMove);

        var patrolState = new NormalPatrolState(this);
        var patrolToMove = new PatrolToChaseTransition<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Move);
        patrolState
             .AddTransition<ENormalPatrolEnemyState>(patrolToMove);

        var moveState = new NormalPatrolChaseStae(this);
        var moveToAttack = new MoveToAttackTransition<ENormalPatrolEnemyState>(this, ENormalPatrolEnemyState.Attack, true);
        moveState
            .AddTransition<ENormalPatrolEnemyState>(moveToAttack);

        var attackState = new LeafFairyAttackState(this);

        AddState(rootState, ENormalPatrolEnemyState.Idle);
        AddState(patrolState, ENormalPatrolEnemyState.Patrol);
        AddState(moveState, ENormalPatrolEnemyState.Move);
        AddState(attackState, ENormalPatrolEnemyState.Attack);
    }

    public IEnumerator Attack(Action act)
    {
        ParticleSystem insPs = Instantiate(attackEffect, attackPoint.position, Quaternion.identity);
        particleOBJ = insPs.gameObject;

        yield return new WaitForSeconds(1f);

        Vector2 dir = (Target.position - attackPoint.position).normalized;
        InstantiateBullet(dir, EEnemyBulletSpeedType.Wind, EEnemyBulletCurveType.Curve90);
        SoundManager.Instance.SFXPlay("InsLeaf", _attackSound, 0.5f);
        act?.Invoke();

        yield return new WaitForSeconds(0.3f);
        insPs.Stop();
        Destroy(insPs.gameObject);
    }


    public void InstantiateBullet(Vector2 dir, EEnemyBulletSpeedType speedType, EEnemyBulletCurveType curveType = EEnemyBulletCurveType.None)
    {
        Instantiate(bullet, attackPoint.position, Quaternion.identity).Shoot(dir, speedType, curveType);
    }

    private void OnDestroy()
    {
        if(particleOBJ != null)
        {
            Destroy(particleOBJ);
        }   
    }

}
