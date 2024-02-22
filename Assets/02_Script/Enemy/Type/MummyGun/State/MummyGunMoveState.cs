using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyGunMoveState : MummyGunRootState
{
    ChaseAction<EMummyGunState> chaseAct;
    public MummyGunMoveState(MummyGunStateController controller) : base(controller)
    {
        chaseAct = new ChaseAction<EMummyGunState>(controller, GameManager.Instance.player, true);
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
