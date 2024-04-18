using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChaseState : NormalRootState
{
    ChaseAction<ENormalEnemyState> chaseAct;

    public NormalChaseState(BaseFSM_Controller<ENormalEnemyState> controller) : base(controller)
    {
        List<SteeringBehaviour> behaviourlist = new List<SteeringBehaviour>()
        {
            new ObstacleAvoidanceBehaviour(controller.transform),
            new SeekBehaviour(controller.transform)
        };
        chaseAct = new ChaseAction<ENormalEnemyState>(controller, behaviourlist, true);
    }

    protected override void EnterState()
    {
        chaseAct.OnEnter();
    }

    protected override void ExitState()
    {
        chaseAct.OnExit();
        controller.StopImmediately();
    }

    protected override void UpdateState()
    {
        base.UpdateState();
        chaseAct.OnUpdate();
    }

}
