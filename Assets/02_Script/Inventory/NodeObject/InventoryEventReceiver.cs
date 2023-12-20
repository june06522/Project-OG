using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public delegate void ReceiveEvent(object parm = null);

public abstract class InventoryEventReceiver : InventoryObjectRoot, IDisposable
{

    protected List<InventoryObjectRoot> connects = new();

    public virtual void Dispose()
    {
    }

    public virtual void Init()
    {
    }

    public override void GetSignal(object parm)
    {

        foreach(var item in connects)
        {

            item.DoGetSignal(parm);

        }

    }

}