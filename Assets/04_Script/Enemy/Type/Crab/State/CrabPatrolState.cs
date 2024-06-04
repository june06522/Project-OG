using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabPatrolState : NormalPatrolRootState
{
    PatrolAction<ENormalPatrolEnemyState> patrolAct;
    public CrabPatrolState(BaseFSM_Controller<ENormalPatrolEnemyState> controller) : base(controller)
    {
        //behaviour
        List<SteeringBehaviour> behaviourlist = new List<SteeringBehaviour>()
        {
            new PatrolBehaviour(controller.transform, patrolRadius: _data.Range),
            new ObstacleAvoidanceBehaviour(controller.transform)
        };

        patrolAct = new PatrolAction<ENormalPatrolEnemyState>(controller, behaviourlist, null);
    }
}
