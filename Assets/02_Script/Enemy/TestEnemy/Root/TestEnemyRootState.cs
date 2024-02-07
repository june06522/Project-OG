using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyRootState : FSM_State<ETestEnemyState>
{
    public TestEnemyDataSO DataSO => _dataSo;
    private TestEnemyDataSO _dataSo;

    public TestEnemyRootState(TestEnemyFSMController controller) : base(controller)
    {
        _dataSo = controller.EnemyData;
    }
}
