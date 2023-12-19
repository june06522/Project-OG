using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InvenNodeRoot : ScriptableObject
{

    public abstract object GetSignal(object signal);

}