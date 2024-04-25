using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemtDieEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;

    protected override void OnInit()
    {
        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnEnemyDie += HandleEnemyDie;

        }

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void HandleEnemyDie()
    {

        SendData s = new SendData(generatorID);

        GetSignal(s);

    }

    public override void Dispose()
    {
        if (PlayerController.EventController != null)
        {
            PlayerController.EventController.OnEnemyDie -= HandleEnemyDie;

        }

    }
}
