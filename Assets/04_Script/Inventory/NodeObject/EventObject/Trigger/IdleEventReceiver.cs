using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;
    public float cool;

    protected override void OnInit()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnIdle += HandleIdle;

        }

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void HandleIdle()
    {

        SendData s = new SendData(generatorID, transform, TriggerID.Idle);

        GetSignal(s);

    }

    public override void Dispose()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnIdle -= HandleIdle;
        }

    }
}
