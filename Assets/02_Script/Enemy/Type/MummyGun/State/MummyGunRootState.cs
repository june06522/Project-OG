using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//root°â patrolState
public class MummyGunRootState : BaseFSM_State<EMummyGunState>
{
    protected new MummyGunStateController controller;
    protected EnemyDataSO _data => controller.EnemyDataSO;

    PatrolAction<EMummyGunState> patrolAct;
    PatrolBehaviour patrolbehaviour;

    protected List<Detector> detectors;

    public MummyGunRootState(MummyGunStateController controller) : base(controller)
    {
        this.controller = controller;
        
        //detector
        detectors = new List<Detector>() 
        { 
            new TargetDetector( controller.transform, _data.ObstacleLayer, _data.TargetAbleLayer),
            new ObstacleDetector( controller.transform, _data.ObstacleLayer),
        };
        
        //behaviour
        List<SteeringBehaviour> behaviourlist = new List<SteeringBehaviour>()
        {
            new PatrolBehaviour(controller.transform, patrolRadius: _data.Range),
            new ObstacleAvoidanceBehaviour(controller.transform)
        };

        patrolAct = new PatrolAction<EMummyGunState>(controller, behaviourlist, controller.DebugTile);
        EnterState();
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
        UpdateDetector();
        patrolAct.OnUpdate();
    }

    public void UpdateDetector()
    {
        foreach(var detector in detectors)
        {
            detector.Detect(controller.AIdata);
        }
    }
}
