using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafFairyPatrolState : LeafFairyRootState
{
    PatrolAction<ELeafFariyState> patrolAct;
    public LeafFairyPatrolState(LeafFairyStateController controller) : base(controller)
    {
        //behaviour
        List<SteeringBehaviour> behaviourlist = new List<SteeringBehaviour>()
        {
            new PatrolBehaviour(controller.transform, patrolRadius: _data.Range),
            new ObstacleAvoidanceBehaviour(controller.transform)
        };

        patrolAct = new PatrolAction<ELeafFariyState>(controller, behaviourlist, null);
    }

    protected override void EnterState()
    {
        //controller.Enemy.enemyAnimController.SetMove(true);
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
