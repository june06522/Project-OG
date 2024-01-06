using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySignalGet : InventoryEventReceiverBase
{

    protected override void OnInit()
    {

        base.OnInit();

        data.OnSignalReceived += GetSignal;

    }

    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

}
