using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;

    protected override void OnInit()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnBasicAttack += HandleBasicAttack;

        }

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void HandleBasicAttack()
    {

        SendData s = new SendData(generatorID, transform);

        GetSignal(s);

    }

    public override void Dispose()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnBasicAttack -= HandleBasicAttack;

        }

    }
}