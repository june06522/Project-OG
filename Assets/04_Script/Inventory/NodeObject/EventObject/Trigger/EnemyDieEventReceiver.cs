using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;
    public int cnt = 1;

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

        SendData s = new SendData(generatorID, transform, TriggerID.Kill, cnt);

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
