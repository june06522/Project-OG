using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPatrolState : NormalPatrolRootState
{
    PatrolAction<ENormalPatrolEnemyState> patrolAct;
    public NormalPatrolState(BaseFSM_Controller<ENormalPatrolEnemyState> controller) : base(controller)
    {
        //behaviour
        List<SteeringBehaviour> behaviourlist = new List<SteeringBehaviour>()
        {
            new PatrolBehaviour(controller.transform, patrolRadius: _data.Range),
            new ObstacleAvoidanceBehaviour(controller.transform)
        };

        patrolAct = new PatrolAction<ENormalPatrolEnemyState>(controller, behaviourlist, null);
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
