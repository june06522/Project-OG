using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEventReceiver : InventoryEventReceiver
{

    public float chargeValue;

    public override void Init()
    {

        if(PlayerController.EventController != null)
        {

            PlayerController.EventController.OnDash += HandleDash;    

        }

    }

    public override void OnReceiveExecute([BindParameterType(typeof(float))] object parm)
    {

        base.OnReceiveExecute(parm);

    }

    private void HandleDash()
    {

        OnReceiveExecute(chargeValue);

    }

    public override void Dispose()
    {
        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnDash -= HandleDash;

        }

    }

}
