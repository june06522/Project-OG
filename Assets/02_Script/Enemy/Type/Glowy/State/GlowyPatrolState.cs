using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowyPatrolState : GlowyRootState
{
    PatrolAction<EGlowyState> patrolAct;
    public GlowyPatrolState(GlowyStateController controller) : base(controller)
    {
        List<SteeringBehaviour> behaviourlist = new List<SteeringBehaviour>()
        {
            new PatrolBehaviour(controller.transform, patrolRadius: _data.Range),
            new ObstacleAvoidanceBehaviour(controller.transform)
        };

        patrolAct = new PatrolAction<EGlowyState>(controller, behaviourlist, null);
    }

    protected override void EnterState()
    {
        patrolAct.OnEnter();
    }

    protected override void ExitState()
    {
        patrolAct.OnExit();
    }

    protected override void UpdateState()
    {
        base.UpdateState();
        patrolAct.OnUpdate();
    }
}
