using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventController : IDisposable
{

    public event Action OnMove;
    public event Action OnDash;
    public event Action OnBasicAttack;
    public event Action OnCool;
    public event Action OnIdle;

    public void OnMoveExecute()         => OnMove?.Invoke();
    public void OnDashExecute()         => OnDash?.Invoke();
    public void OnBasicAttackExecute()  => OnBasicAttack?.Invoke();
    public void OnCoolExecute()         => OnCool?.Invoke();
    public void OnIdleExecute()         => OnIdle?.Invoke();

    public void Dispose()
    {

        OnMove = null;
        OnDash = null;
        OnBasicAttack = null;
        OnCool = null;
        OnIdle = null;

    }
}
