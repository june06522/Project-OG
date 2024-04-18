using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalSendObject : InventoryExecuterBase
{
    public override void GetSignal(object signal)
    {

        data.SendSignal(signal);

    }

}
