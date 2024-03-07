using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyGunPatrolState : MummyGunRootState
{
    PatrolAction<EMummyGunState> patrolAct;
    public MummyGunPatrolState(MummyGunStateController controller) : base(controller)
    {
        //behaviour
        List<SteeringBehaviour> behaviourlist = new List<SteeringBehaviour>()
        {
            new PatrolBehaviour(controller.transform, patrolRadius: _data.Range),
            new ObstacleAvoidanceBehaviour(controller.transform)
        };

        patrolAct = new PatrolAction<EMummyGunState>(controller, behaviourlist, controller.DebugTile);
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
