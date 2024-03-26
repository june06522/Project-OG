using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateBoss : Boss
{
    public GameObject G_slateOnlyCollector;

    public List<Material> L_materials;

    public float F_minimiAwayDistance;

    private enum BossState
    {
        Idle,
        FullHP,
        HalfHP,
        Dead
    }

    private BossState _curBossState;

    private BossFSM _bossFSM;

    void Start()
    {
        _curBossState = BossState.Idle;
        _bossFSM = new BossFSM(new BossIdleState(this));

        ChangeState();
    }

    protected override void Update()
    {
        base.Update();

        ChangeState();
        _bossFSM.UpdateBossState();
    }

    private void ChangeState()
    {
        switch(_curBossState)
        {
            case BossState.Idle:
                ChangeBossState(BossState.FullHP);
                break;
            case BossState.FullHP:
                break;
            case BossState.HalfHP:
                break;
        }
    }

    private void ChangeBossState(BossState nextBossState)
    {
        _curBossState = nextBossState;

        switch(_curBossState)
        {
            case BossState.FullHP:
                _bossFSM.ChangeBossState(new SFullHPState(this));
                break;
            case BossState.HalfHP:
                _bossFSM.ChangeBossState(new SHalfState(this));
                break;
            case BossState.Dead:
                _bossFSM.ChangeBossState(new SDeadState(this));
                break;
        }
    }

    public void LaserReturnAll()
    {
        int childCount = 0;
        GameObject[] objs;

        childCount = G_slateOnlyCollector.transform.childCount;
        objs = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            objs[i] = G_slateOnlyCollector.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < childCount; i++)
        {
            if (objs[i] == null)
                continue;
            ObjectPool.Instance.ReturnObject(objs[i]);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
    }
}
