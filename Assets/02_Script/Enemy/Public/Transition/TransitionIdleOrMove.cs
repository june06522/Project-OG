using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공용 Transition
/// </summary>

public class TransitionIdleOrMove<T> : BaseFSM_Transition<T> where T : Enum
{
    enum CheckType
    {
        Idle,
        Move
    }

    Transform playerTrm;
    EnemyDataSO _data;
    CheckType _myType;

    public TransitionIdleOrMove(BaseFSM_Controller<T> controller, T nextState) : base(controller, nextState)
    {

        if (nextState.ToString() == "Idle")
            _myType = CheckType.Idle;
        else if (nextState.ToString() == "Move")
            _myType = CheckType.Move;
        else
            Debug.LogError("Only (idle, Move) can!");

        
        playerTrm = GameManager.Instance.player;

        _data = controller.EnemyData;
        
        #region enum이름으로 데이터 할당
        //string className = $"{nextState.GetType().Name.Remove(1,1)}Controller";
        //Debug.Log("ClassName : " + className);
        //Type type = Type.GetType($"{className}, Assembly-CSharp");
        //if (type == null)
        //{
        //    Debug.LogError("class이름이 enum 이름과 통일되지 않음");
        //}
        //else
        //{
        //    var enemyDataProperty = type.GetProperty("EnemyData");
        //    if(enemyDataProperty != null)
        //        _data = enemyDataProperty.GetValue(controller) as EnemyDataSO;
        //    else
        //        Debug.LogError("EnemyData property not found in the dynamically obtained type");

        //}
        #endregion
    }

    protected override bool CheckTransition()
    {
        Debug.Log($"nextState : {nextState}");
        //_nextState = _nextState;
        if(_myType == CheckType.Idle) // case: idle로.
        {

            if(_data.CheckObstacle) // 땅
            {
                //감지거리 밖에 있거나 장애물이 있으면 전환.
                return !Transitions.CheckDistance(playerTrm, this.transform, _data.Range) ||
                    Transitions.CheckObstacleBetweenTarget(playerTrm, this.transform, EObstacleType.Wall);
            }
            else // 공중
            {
                //감지거리 밖에 있으면 전환.
                return !Transitions.CheckDistance(playerTrm, this.transform, _data.Range);
            }

        }
        else if (_myType == CheckType.Move) // case: move로.
        {
            if (_data.CheckObstacle) // 땅
            {
                //감지거리 안에 있고 사이에 장애물이 없으면 전환
                return Transitions.CheckDistance(playerTrm, this.transform, _data.Range) &&
                        !Transitions.CheckObstacleBetweenTarget(playerTrm, this.transform, EObstacleType.Wall);
            }
            else // 공중
            {
                //감지거리 안에 있으면 전환
                return Transitions.CheckDistance(playerTrm, this.transform, _data.Range);
            }
        }
        else
        {
            Debug.LogError("enum sequence was wrong. //idle : 0, move : 1");
        }

        return false;
    }
}
