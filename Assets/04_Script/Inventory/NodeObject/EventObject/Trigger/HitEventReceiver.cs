
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;

    protected override void OnInit()
    {
        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnHit += HandleHit;

        }

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void HandleHit()
    {

        SendData s = new SendData(generatorID,transform,TriggerID.GetHit);

        GetSignal(s);

    }

    public override void Dispose()
    {
        if (PlayerController.EventController != null)
        {
            PlayerController.EventController.OnHit -= HandleHit;

        }

    }
}
