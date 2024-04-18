using System.Collections.Generic;

public class NormalRootState : BaseFSM_State<ENormalEnemyState>
{
    protected EnemyDataSO _data => controller.EnemyDataSO;
    protected List<Detector> detectors;

    public NormalRootState(BaseFSM_Controller<ENormalEnemyState> controller) : base(controller)
    {
        //detector
        if(_data.CheckObstacle)
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
    