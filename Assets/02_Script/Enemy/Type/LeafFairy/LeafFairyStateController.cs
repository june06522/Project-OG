using FSM_System;
using System;
using System.Collections;
using UnityEngine;

public enum ELeafFariyState
{
    Idle = 0,
    Patrol = 1,
    Move = 2,
    Attack,
    Dead,
}

public class LeafFairyStateController : BaseFSM_Controller<ELeafFariyState>
{
    [SerializeField]
    public ParticleSystem attackEffect;
    [SerializeField]
    public Transform attackPoint;
    [SerializeField]
    public LeafBullet bullet;

    GameObject particleOBJ;
    protected override void Start()
    {
        base.Start();

        var rootState = new LeafFairyRootState(this);
        var rootToMove = new RoomOpenTransitions<ELeafFariyState>(this, ELeafFariyState.Patrol); 
        rootState
            .AddTransition<ELeafFariyState>(rootToMove);

        var patrolState = new LeafFairyPatrolState(this);
        var patrolToMove = new PatrolToChaseTransition<ELeafFariyState>(this, ELeafFariyState.Move);
        patrolState
             .AddTransition<ELeafFariyState>(patrolToMove);

        var moveState = new LeafFairyMoveState(this);
        var moveToAttack = new MoveToAttackTransition<ELeafFariyState>(this, ELeafFariyState.Attack, true);
        moveState
            .AddTransition<ELeafFariyState>(moveToAttack);

        var attackState = new LeafFairyAttackState(this);

        AddState(rootState, ELeafFariyState.Idle);
        AddState(patrolState, ELeafFariyState.Patrol);
        AddState(moveState, ELeafFariyState.Move);
        AddState(attackState, ELeafFariyState.Attack);
    }

    public IEnumerator Attack(Action act)
    {
        ParticleSystem insPs = Instantiate(attackEffect, attackPoint.position, Quaternion.identity);
        particleOBJ = insPs.gameObject;

        yield return new WaitForSeconds(1f);

        Vector2 dir = (Target.position - attackPoint.position).normalized;
        InstantiateBullet(dir, EEnemyBulletSpeedType.Wind, EEnemyBulletCurveType.Curve90);
        
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
