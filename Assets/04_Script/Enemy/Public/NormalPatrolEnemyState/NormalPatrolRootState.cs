using FSM_System;
using System.Collections.Generic;

public class NormalPatrolRootState : BaseFSM_State<ENormalPatrolEnemyState>
{
    protected EnemyDataSO _data => controller.EnemyDataSO;

    protected List<Detector> detectors;

    public NormalPatrolRootState(BaseFSM_Controller<ENormalPatrolEnemyState> controller) : base(controller)
    {
        //detector
        if (_data.CheckObstacle)
        {
            detectors = new List<Detector>()
            {
                new TargetDetector( controller.transform, _data),
                new ObstacleDetector( controller.transform, _data.ObstacleLayer),
            };
        }
        else
        {
            detectors = new List<Detector>()
            {
                new TargetDetector( controller.transform, _data),
            };
        }
    }

    protected override void EnterState()
    {
        controller.Enemy.enemyAnimController.SetMove(false);
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
