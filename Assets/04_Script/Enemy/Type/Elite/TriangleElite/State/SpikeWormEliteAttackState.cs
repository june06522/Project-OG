using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;

public class SpikeWormEliteAttackState : FSM_State<ENormalEnemyState>
{
    private SpikeWormEliteStateController _controller;

    public SpikeWormEliteAttackState(SpikeWormEliteStateController controller) : base(controller)
    {
        _controller = controller;
    }

    protected override void EnterState()
    {
        
    }

    protected override void ExitState()
    {
        
    }

    protected override void UpdateState()
    {
        
    }
}
