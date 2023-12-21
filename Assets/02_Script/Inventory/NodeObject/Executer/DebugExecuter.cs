using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugExecuter : InventoryExecuterBase
{
    public override void GetSignal(object signal)
    {

        Debug.Log(signal);

    }

}
