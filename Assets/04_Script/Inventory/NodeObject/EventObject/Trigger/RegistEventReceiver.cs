using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;
    protected override void OnInit()
    {
        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnRegist += HandleRegist;

        }

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void HandleRegist()
    {

        SendData s = new SendData(generatorID, transform, TriggerID.Regist);

        GetSignal(s);

    }

    public override void Dispose()
    {
        if (PlayerController.EventController != null)
        {
            PlayerController.EventController.OnRegist -= HandleRegist;

        }

    }
}
