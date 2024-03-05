using FSM_System;

public class BatEnemyRootState : FSM_State<EBatState>
{
    protected new BatStateController controller;
    protected EnemyDataSO _data => controller.EnemyDataSO;
    TargetDetector detector;

    public BatEnemyRootState(BatStateController controller) : base(controller)
    {
        this.controller = controller;
        detector = new TargetDetector(controller.transform, _data);
    }

    protected override void UpdateState()
    {
        UpdateDetector();
    }

    public void UpdateDetector()
    {
        detector.Detect(controller.AIdata);   
    }
}
