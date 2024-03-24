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
        if(!_boss.B_isStop)
        {
            Vector2 dir = GameManager.Instance.player.transform.position - _boss.transform.position;
        }
    }
}
