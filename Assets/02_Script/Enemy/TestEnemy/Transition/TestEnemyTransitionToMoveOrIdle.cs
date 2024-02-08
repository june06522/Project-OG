using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyTransitionToMoveOrIdle : FSM_Transition<ETestEnemyState>
{
    private Transform playerTrm;
    private TestEnemyDataSO _data;
    private ETestEnemyState _nextState;

    public TestEnemyTransitionToMoveOrIdle(TestEnemyFSMController controller, ETestEnemyState nextState) : base(controller, nextState)
    {
        playerTrm = GameManager.Instance.player.transform;
        _data = controller.EnemyData as TestEnemyDataSO;
        _nextState = nextState;
    }


    protected override bool CheckTransition()
    {
        switch (_nextState)
        {
            case ETestEnemyState.Move:
                return Transitions.CheckDistance(playerTrm, this.transform, _data.Range) &&
                       !Transitions.CheckObstacleBetweenTarget(playerTrm, this.transform, EObstacleType.Wall);
            case ETestEnemyState.Idle:
                return !Transitions.CheckDistance(playerTrm, this.transform, _data.Range) && 
                        Transitions.CheckObstacleBetweenTarget(playerTrm, this.transform, EObstacleType.Wall);
        }
        return false;
    }
}
