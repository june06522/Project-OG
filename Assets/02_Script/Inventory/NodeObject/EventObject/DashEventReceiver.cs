using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEventReceiver : InventoryEventReceiverBase
{

    public float chargeValue;

    protected override void OnInit()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnDash += HandleDash;

        }

    }

    [BindExecuteType(typeof(Skill))]
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
