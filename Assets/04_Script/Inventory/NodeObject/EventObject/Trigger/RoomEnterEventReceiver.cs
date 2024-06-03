using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;

    protected override void OnInit()
    {
        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnRoomEnter += HandleRoomEnter;

        }

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void HandleRoomEnter()
    {

        SendData s = new SendData(generatorID, transform, TriggerID.RoomClear);

        GetSignal(s);

    }

    public override void Dispose()
    {
        if (PlayerController.EventController != null)
        {
            PlayerController.EventController.OnRoomEnter -= HandleRoomEnter;

        }

    }
}
