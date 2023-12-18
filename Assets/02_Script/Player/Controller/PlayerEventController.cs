using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventController : IDisposable
{

    public event Action OnMove;
    public event Action OnDash;

    public void OnMoveExecute() => OnMove?.Invoke();
    public void OnDashExecute() => OnDash?.Invoke();

    public void Dispose()
    {

        OnMove = null;
        OnDash = null;

    }


}
