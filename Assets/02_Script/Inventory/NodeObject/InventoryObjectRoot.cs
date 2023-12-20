using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryObjectRoot : ScriptableObject
{

    public void DoGetSignal(object signal)
    {

        if(signal == null) return;
        GetSignal(signal);

    }

    public abstract void GetSignal(object signal);

}