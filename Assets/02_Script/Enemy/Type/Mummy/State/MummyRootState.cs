
using System.Collections.Generic;

//root°â patrolState
public class MummyRootState : BaseFSM_State<EMummyState>
{
    protected EnemyDataSO _data => controller.EnemyDataSO;

    PatrolAction<EMummyState> patrolAct;
    protected List<Detector> detectors;

    public MummyRootState(BaseFSM_Controller<EMummyState> controller) : base(controller)
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
        foreach (var detector in detectors)
        {
            detector.Detect(controller.AIdata);
        }
    }
}
    