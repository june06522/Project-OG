using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyPatrolState : MummyRootState
{
    PatrolAction<EMummyState> patrolAct;
    public MummyPatrolState(BaseFSM_Controller<EMummyState> controller) : base(controller)
    {
        //behaviour
        List<SteeringBehaviour> behaviourlist = new List<SteeringBehaviour>()
        {
            new PatrolBehaviour(controller.transform, patrolRadius: _data.Range),
            new ObstacleAvoidanceBehaviour(controller.transform)
        };

        patrolAct = new PatrolAction<EMummyState>(controller, behaviourlist, null);
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
