using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombStateController : BaseFSM_Controller<ENormalEnemyState>
{
    public bool explodeDebug = false;

    public ParticleSystem bombAnim;
    public GameObject showRange;
    public float radius = 3f;
    private GameObject instShowRangeObj;
    [SerializeField]
    private AudioClip _bombClip;

    protected override void Start()
    {
        base.Start();

        var rootState = new NormalRootState(this);
        var rootToChase = new TransitionIdleOrMove<ENormalEnemyState>(this, ENormalEnemyState.Move);
        rootState.AddTransition(rootToChase);

        var chaseState = new NormalChaseState(this);
        var chaseToAttack = new MoveToAttackTransition<ENormalEnemyState>(this, ENormalEnemyState.Attack, true);
        chaseState
            .AddTransition<ENormalEnemyState>(chaseToAttack);

        var attackState = new BombAttackState(this);

        AddState(rootState, ENormalEnemyState.Idle);
        AddState(chaseState, ENormalEnemyState.Move);
        AddState(attackState, ENormalEnemyState.Attack);
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

        Instantiate(bombAnim, transform.position, Quaternion.identity).Play();

        if(instShowRangeObj != null)
            Destroy(instShowRangeObj);
        Enemy.Die();
    }

    public void InstantiateRange()
    {
        SoundManager.Instance.SFXPlay("bomb", _bombClip, 0.2f);
        instShowRangeObj = Instantiate(showRange, transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        if (instShowRangeObj != null)
            Destroy(instShowRangeObj);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(explodeDebug)
        {
            Gizmos.color = new Color(1, 0, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
#endif
}
