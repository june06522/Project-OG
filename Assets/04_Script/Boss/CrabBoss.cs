using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspector에서 보이는 변수들
public partial class CrabBoss
{
    public GameObject crabLaserCollector;
    public GameObject crabLeftNipper;
    public GameObject crabRightNipper;
    public GameObject leftJoints;
    public GameObject rightJoints;

    [Header("Transform")]
    public Transform leftFirePos;
    public Transform rightFirePos;
    public Transform leftGuidePos;
    public Transform rightGuidePos;
    public Transform mouthFirePos;

    [Header("Animator")]
    public Animator animator;
}

public partial class CrabBoss : Boss
{
    [HideInInspector] public readonly int laserShooting = Animator.StringToHash("IsLaserShooting");
    [HideInInspector] public readonly int swing = Animator.StringToHash("IsSwing");
    [HideInInspector] public readonly int bubbleShooting = Animator.StringToHash("IsBubbleShooting");
    [HideInInspector] public readonly int leftPunching = Animator.StringToHash("IsLeftPunching");

    private BossState _curBossState;

    private BossFSM _bossFSM;

    private CrabPattern _pattern;

    private void Awake()
    {
        _pattern = GetComponent<CrabPattern>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _curBossState = BossState.Idle;
        _bossFSM = new BossFSM(new CIdleState(this, _pattern));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Update()
    {
        base.Update();

        ChangeState();

        _bossFSM.UpdateBossState();
    }

    private void BodyAnimation()
    {

    }

    private void ChangeState()
    {
        switch (_curBossState)
        {
            case BossState.Idle:
                ChangeBossState(BossState.Alive);
                break;
            case BossState.Alive:
                if(IsDie)
                {
                    ChangeBossState(BossState.Dead);
                }
                break;
        }
    }

    private void ChangeBossState(BossState nextBossState)
    {
        _curBossState = nextBossState;

        switch (_curBossState)
        {
            case BossState.Alive:
                _bossFSM.ChangeBossState(new CAliveState(this, _pattern));
                break;
            case BossState.Dead:
                _bossFSM.ChangeBossState(new CDeadState(this, _pattern));
                break;
        }
    }
}
