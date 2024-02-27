using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//root°â patrolState
public class MummyGunRootState : BaseFSM_State<EMummyGunState>
{
    protected new MummyGunStateController controller;
    protected EnemyDataSO _data => controller.EnemyDataSO;

    PatrolAction<EMummyGunState> patrolAct;

    protected List<Detector> detectors;

    public MummyGunRootState(MummyGunStateController controller) : base(controller)
    {
        this.controller = controller;
        detectors = new List<Detector>() 
        { 
            new TargetDetector( controller.transform, _data.ObstacleLayer, _data.TargetAbleLayer),
            new ObstacleDetector( controller.transform, _data.ObstacleLayer),
        };
        patrolAct = new PatrolAction<EMummyGunState>(controller, controller.DebugTile);
        
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
