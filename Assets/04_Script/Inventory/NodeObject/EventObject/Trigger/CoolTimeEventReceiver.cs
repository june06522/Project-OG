using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTimeEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;
    public double cool;

    protected override void OnInit()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnCool += HandleCool;

        }

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void HandleCool()
    {

        SendData s = new SendData(generatorID, transform, TriggerID.CoolTime, cool);

        GetSignal(s);

    }

    public override void Dispose()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnCool -= HandleCool;

        }

    }
}
