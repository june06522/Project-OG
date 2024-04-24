using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public delegate void ReceiveEvent(object parm = null);

//이벤트가 실행됐을때 작동하는 얘 ex)대쉬
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

    private void OnDestroy()
    {

        Debug.Log("구독해제2222");
    }

}