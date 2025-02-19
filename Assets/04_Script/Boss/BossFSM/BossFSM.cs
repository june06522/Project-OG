using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSM
{
    private BossBaseState _curBossState;

    public BossFSM(BossBaseState initBossState)
    {
        ChangeBossState(initBossState);
    }

    public void ChangeBossState(BossBaseState nextBossState)
    {
        if (nextBossState == _curBossState)
            return;

        if (_curBossState != null)
            _curBossState.OnBossStateExit();

        _curBossState = nextBossState;
        _curBossState.OnBossStateOn();
    }

    public void UpdateBossState()
    {
        if (_curBossState != null)
            _curBossState.OnBossStateUpdate();
    }
}
