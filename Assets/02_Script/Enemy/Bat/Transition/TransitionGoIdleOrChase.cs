using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionGoIdleOrChase<T> : FSM_Transition<T> where T : Enum
{
    T _nextState;
    Transform playerTrm;
    EnemyDataSO _data;

    public TransitionGoIdleOrChase(FSM_Controller<T> controller, T nextState) : base(controller, nextState)
    {
        _nextState = nextState;
        playerTrm = GameManager.Instance.player;

        string className = $"{nextState.GetType().Name.Remove(1,1)}Controller";
        Debug.Log("ClassName : " + className);
        Type type = Type.GetType($"{className}, Assembly-CSharp");
        if(type == null)
        {
            Debug.LogError("class이름이 enum 이름과 통일되지 않음");
        }
        else
            _data = (controller as typeof(className).EnemyData;
    }

    protected override bool CheckTransition()
    {
        switch (_nextState)
        {
            case .Chase:
                return Transitions.CheckDistance(playerTrm, this.transform, _data.Range) &&
                       !Transitions.CheckObstacleBetweenTarget(playerTrm, this.transform, EObstacleType.Wall);
            case EBatState.Idle:
                return !Transitions.CheckDistance(playerTrm, this.transform, _data.Range) &&
                        Transitions.CheckObstacleBetweenTarget(playerTrm, this.transform, EObstacleType.Wall);
        }
        return false;
    }
}
