using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;


public enum UngonState
{
    Idle,
    Jump,
    Fire,
}


public class UngonController : FSM_Controller<UngonState>
{
    [SerializeField] UngonDataSO _data;

    protected override void Awake()
    {
        _data = Instantiate(_data);
    }
}

public class UngonIdleState<T> : FSM_State<T> where T : System.Enum
{
    public UngonIdleState(FSM_Controller<T> controller, EnemyDataSO data) : base(controller)
    {

    }

    protected override void UpdateState()
    {

    }
}


public class UngonMoveState<T> : FSM_State<T> where T : System.Enum
{
    public UngonMoveState(T idleState, FSM_Controller<T> controller, EnemyDataSO data) : base(controller)
    {
        //_idleState = idleState;
    }
}