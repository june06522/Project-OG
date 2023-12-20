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

    [BindExecuteType(typeof(float))]
    public override void GetSignal(object parm)
    {
        
        base.GetSignal(parm);

    }

    private void HandleDash()
    {

        GetSignal(chargeValue);

    }

    public override void Dispose()
    {
        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnDash -= HandleDash;

        }

    }

}
