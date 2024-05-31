using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;

    protected override void OnInit()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnMove += HandleMove;

        }

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void HandleMove()
    {

        SendData s = new SendData(generatorID, transform, TriggerID.Move);

        GetSignal(s);

    }

    public override void Dispose()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnMove -= HandleMove;
        }

    }

    private new void OnDestroy()
    {
        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnMove -= HandleMove;

        }
    }
}
