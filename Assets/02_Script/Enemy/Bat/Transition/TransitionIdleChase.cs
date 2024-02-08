using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionIdleChase<T> : FSM_Transition<T> where T : Enum
{
    T _nextState;
    Transform playerTrm;
    EnemyDataSO _data;

    public TransitionIdleChase(FSM_Controller<T> controller, T nextState) : base(controller, nextState)
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
        //else
       //     _data = (controller as ().EnemyData;
    }

    protected override bool CheckTransition()
    {
        return true;
        //switch (_nextState)
        //{
        //    case .Chase:
        //        return Transitions.CheckDistance(playerTrm, this.transform, _data.Range) &&
        //               !Transitions.CheckObstacleBetweenTarget(playerTrm, this.transform, EObstacleType.Wall);
        //    case EBatState.Idle:
        //        return !Transitions.CheckDistance(playerTrm, this.transform, _data.Range) &&
        //                Transitions.CheckObstacleBetweenTarget(playerTrm, this.transform, EObstacleType.Wall);
        //}
        //return false;
    }
}
