using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMoveState : BatEnemyRootState
{
    ChaseAction<EBatState> chaseAct;
    public BatMoveState(BatStateController controller) : base(controller)
    {
        chaseAct = new ChaseAction<EBatState>( controller, GameManager.Instance.player);
    }

    protected override void EnterState()
    {
        chaseAct.OnEnter();
    }

    protected override void ExitState()
    {
        chaseAct.OnExit();
    }

    protected override void UpdateState()
    {
        chaseAct.OnUpdate();
    }
}
