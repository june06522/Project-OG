using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryObjectRoot : ScriptableObject
{

    public abstract object GetSignal(object signal);

}