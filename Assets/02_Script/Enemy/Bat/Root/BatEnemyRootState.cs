using FSM_System;

public class BatEnemyRootState : FSM_State<EBatState>
{
    public EnemyDataSO DataSO => _dataSo;
    private EnemyDataSO _dataSo;

    public BatEnemyRootState(BatStateController controller) : base(controller)
    {
        _dataSo = controller.EnemyData;
    }
}
