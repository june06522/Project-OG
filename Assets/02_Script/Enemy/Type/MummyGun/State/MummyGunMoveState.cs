using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyGunMoveState : MummyGunRootState
{
    ChaseAction<EMummyGunState> chaseAct;
    public MummyGunMoveState(MummyGunStateController controller) : base(controller)
    {
        List<SteeringBehaviour> behaviourlist = new List<SteeringBehaviour>() 
        {
            new ObstacleAvoidanceBehaviour(controller.transform),
            new SeekBehaviour(controller.transform)
        };
        chaseAct = new ChaseAction<EMummyGunState>(controller, 
                                        controller.AIdata.currentTarget, behaviourlist, true);
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
        UpdateDetector();
        chaseAct.OnUpdate();
    }
}
