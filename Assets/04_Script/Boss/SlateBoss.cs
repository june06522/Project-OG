using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspector에서 보이는 변수들
public partial class SlateBoss
{
    public GameObject slateOnlyCollector;
    public GameObject minimisPositions;
    public GameObject bigestBody;
    public GameObject mediumSizeBody;
    public GameObject smallestBody;

    [field: Header("AudioClip")]
    public AudioClip deadClip;
    public AudioClip laserClip;
    public AudioClip fireClip;

    [field: Header("Material")]
    public Material basicMat;
    public Material minimiBasicMat;
    public Material warningMat;
    public Material laserMat;
    public Material hologramMinimiMat;
    public Material StopMat;

    [Header("LineRenderer")]
    public LineRenderer line;

    [Header("SlateOnly")]
    public float minimiAwayDistance;
    [SerializeField]
    private int _minimiCount;
}

public partial class SlateBoss : Boss
{
    public int MinimiCount { get => _minimiCount; set => _minimiCount = value; }

    [HideInInspector] public bool fullHP;
    [HideInInspector] public bool halfHP;

    private SlatePattern _pattern;

    private BossState _curBossState;

    private BossFSM _bossFSM;

    protected override void OnEnable()
    {
        base.OnEnable();

        bossColor = new Color(1, 0.1960784f, 0.427451f);

        transform.gameObject.layer = LayerMask.NameToLayer("Boss");
        _pattern = GetComponent<SlatePattern>();
        fullHP = true;
        halfHP = false;
        isIdle = true;

        _curBossState = BossState.Idle;
        _bossFSM = new BossFSM(new SIdleState(this, _pattern));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Update()
    {
        base.Update();

        if (IsDie && !isDead)
        {
            fullHP = false;
            halfHP = false;
            isStop = true;
            ChangeBossState(BossState.Dead);
        }

        if (!isDead)
        {
            ChangeState();

            _bossFSM.UpdateBossState();
        }
    }

    private void ChangeState()
    {
        switch (_curBossState)
        {
            case BossState.Idle:
                if(!isIdle)
                {
                    ChangeBossState(BossState.FullHP);
                }
                break;
            case BossState.FullHP:
                if (_currentHP < so.MaxHP / 2)
                {
                    ChangeBossState(BossState.HalfHP);
                }
                break;
            case BossState.HalfHP:
                if (IsDie)
                {
                    ChangeBossState(BossState.Dead);
                }
                break;
        }
    }

    private void ChangeBossState(BossState nextBossState)
    {
        _curBossState = nextBossState;

        switch(_curBossState)
        {
            case BossState.FullHP:
                _bossFSM.ChangeBossState(new SFullHPState(this, _pattern));
                break;
            case BossState.HalfHP:
                _bossFSM.ChangeBossState(new SHalfHPState(this, _pattern));
                break;
            case BossState.Dead:
                isDead = true;
                _bossFSM.ChangeBossState(new SDeadState(this, _pattern));
                break;
        }
    }

    public void ReturnMinimi(GameObject[] objs)
    {
        for (int i = 0; i < objs.Length; i++)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.SlateMinimi, objs[i]);
        }
    }

    public void LaserReturnAll()
    {
        int childCount = 0;
        GameObject[] objs;

        childCount = slateOnlyCollector.transform.childCount;
        objs = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            objs[i] = slateOnlyCollector.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < childCount; i++)
        {
            if (objs[i] == null)
                continue;
            ObjectPool.Instance.ReturnObject(objs[i]);
        }
    }
}
