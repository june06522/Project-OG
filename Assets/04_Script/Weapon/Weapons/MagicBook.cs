using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBook : InvenWeapon
{
    public override void Attack(Transform target)
    {



    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {



    }
}
