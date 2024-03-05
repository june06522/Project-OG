using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public delegate void ReceiveEvent(object parm = null);

// 이벤트가 실행 됐을떄 작동하는 애 ex) 대쉬
public abstract class InventoryEventReceiverBase : InventoryObjectRoot, IDisposable
{

    public virtual void Dispose()
    {

    }

    public override void GetSignal(object parm)
    {

        foreach(var item in connectedOutput)
        {

            item.DoGetSignal(parm);

        }

    }

}