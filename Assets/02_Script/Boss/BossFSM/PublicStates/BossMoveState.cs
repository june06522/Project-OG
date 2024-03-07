using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveState : BossBaseState
{
    public BossMoveState(Boss boss) : base(boss)
    {
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        
    }

    public override void OnBossStateUpdate()
    {
        if(!_boss.isStop)
        {
            Vector2 dir = _boss.player.transform.position - _boss.transform.position;

            _boss.rigid.velocity = dir.normalized * _boss.bossSo.Speed;
        }
    }
}