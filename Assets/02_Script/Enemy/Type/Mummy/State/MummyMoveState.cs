using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyMoveState : MummyRootState
{
    ChaseAction<EMummyState> chaseAct;
    public MummyMoveState(MummyStateController controller) : base(controller)
    {
        chaseAct = new ChaseAction<EMummyState>(controller, GameManager.Instance.player, true);
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
