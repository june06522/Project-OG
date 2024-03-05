using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//root°â patrolState
public class MummyGunRootState : BaseFSM_State<EMummyGunState>
{
    protected new MummyGunStateController controller;
    protected EnemyDataSO _data => controller.EnemyDataSO;

    protected List<Detector> detectors;

    public MummyGunRootState(MummyGunStateController controller) : base(controller)
    {
        this.controller = controller;
        
        //detector
        detectors = new List<Detector>() 
        { 
            new TargetDetector( controller.transform, _data),
            new ObstacleDetector( controller.transform, _data.ObstacleLayer),
        };
    }

    protected override void UpdateState()
    {
        UpdateDetector();
    }

    public void UpdateDetector()
    {
        foreach(var detector in detectors)
        {
            detector.Detect(controller.AIdata);
        }
    }
}
