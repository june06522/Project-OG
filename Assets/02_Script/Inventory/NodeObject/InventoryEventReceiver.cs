using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ReceiveEvent(object parm = null);

public abstract class InventoryEventReceiver : ScriptableObject, IDisposable
{
    
    public event ReceiveEvent OnReceived;

    public virtual void Dispose()
    {
    }

    public virtual void Init()
    {
    }

    public virtual void OnReceiveExecute(object parm = null)
    {

        OnReceived?.Invoke(parm);

    }

}