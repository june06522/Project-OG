using FSM_System;

public class BatEnemyRootState : FSM_State<EBatState>
{
    protected new BatStateController controller;
    protected EnemyDataSO _data => controller.EnemyDataSO;

    public BatEnemyRootState(BatStateController controller) : base(controller)
    {
        this.controller = controller;
    }
}
