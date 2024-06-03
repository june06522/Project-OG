using DG.Tweening;
using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombStateController : BaseFSM_Controller<ENormalEnemyState>
{
    public bool explodeDebug = false;

    public ParticleSystem bombParticle;
    public ParticleSystem bombParticle2;
    public EnemyBullet bombBullet;

    public GameObject showRange;
    public float radius = 3f;
    private GameObject instShowRangeObj;
    [SerializeField]
    private AudioClip _bombClip;
    [SerializeField]
    private AudioClip _dangerClip;

    [SerializeField]
    private Collider2D _col;
    float _angle = 0;

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
        if(bombParticle2 != null)
            Instantiate(bombParticle2, transform.position, Quaternion.identity).Play();

        if(_bombClip != null)
            SoundManager.Instance.SFXPlay("bomb", _bombClip, 0.6f);
        CameraManager.Instance.CameraShake(0.2f, 0.2f);

        if(bombBullet != null)
        {
            for (int i = 0; i < 4; ++i)
            {
                float angle = (_angle + i * 90) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Instantiate(bombBullet, instShowRangeObj.transform.position, Quaternion.identity).Shoot(dir);
            }
        }

        if(instShowRangeObj != null)
            Destroy(instShowRangeObj);

        Enemy.Die();
    }

    public void InstantiateWarning()
    {
        if(_col != null)
            _col.enabled = false;

        _angle = Random.Range(0f, 90f);
        if(bombParticle != null)
            Instantiate(bombParticle, transform.position, Quaternion.identity).Play();
        SoundManager.Instance.SFXPlay("danger", _dangerClip, 0.5f);
        instShowRangeObj = Instantiate(showRange, transform.position, Quaternion.identity);
        instShowRangeObj.transform.eulerAngles = new Vector3(0f, 0f, _angle);
        instShowRangeObj.transform.localScale = Vector3.zero;
        instShowRangeObj.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutElastic);
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
