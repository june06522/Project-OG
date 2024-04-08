using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBoss : Boss
{
    public GameObject FlowerOnlyCollector;

    private BossState _curBossState;

    private BossFSM _bossFSM;

    private FlowerPattern _pattern;

    protected override void OnEnable()
    {
        base.OnEnable();

        _pattern = GetComponent<FlowerPattern>();
        _bossFSM = new BossFSM(new BossIdleState(this, _pattern));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Update()
    {
        base.Update();

        ChangeState();
    }

    private void ChangeState()
    {
        switch (_curBossState)
        {
            case BossState.Idle:
                ChangeBossState(BossState.Flowering);
                break;
            //case BossState.Flowering:
            //    ChangeBossState(BossState.FullBloom);
            //    break;
            //case BossState.FullBloom:
            //    ChangeBossState(BossState.Withered);
            //    break;
            //case BossState.Withered:
            //    if(IsDie)
            //    {
            //        ChangeBossState(BossState.Dead);
            //    }
            //    break;
        }
    }

    private void ChangeBossState(BossState nextBossState)
    {
        _curBossState = nextBossState;

        switch (_curBossState)
        {
            case BossState.Idle:
                _bossFSM.ChangeBossState(new BossIdleState(this, _pattern));
                break;
            case BossState.Flowering:
                _bossFSM.ChangeBossState(new FloweringState(this, _pattern));
                break;
            case BossState.FullBloom:
                _bossFSM.ChangeBossState(new FullBloomState(this, _pattern));
                break;
            case BossState.Withered:
                _bossFSM.ChangeBossState(new WitheredState(this, _pattern));
                break;
            case BossState.Dead:
                isDead = true;
                _bossFSM.ChangeBossState(new FDeadState(this, _pattern));
                break;
        }
    }
}
