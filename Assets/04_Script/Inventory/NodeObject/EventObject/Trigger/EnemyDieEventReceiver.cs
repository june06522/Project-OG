using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;
    public int cnt = 1;
    private int curdie = 0;

    protected override void OnInit()
    {
        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnEnemyDie += EnemyDie;

        }

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void EnemyDie()
    {
        curdie++;
        if (curdie >= cnt)
        {
            curdie -= cnt;
            HandleEnemyDie();
        }
    }

    private void HandleEnemyDie()
    {

        SendData s = new SendData(generatorID, transform);

        GetSignal(s);

    }

    public override void Dispose()
    {
        if (PlayerController.EventController != null)
        {
            PlayerController.EventController.OnEnemyDie -= EnemyDie;

        }

    }
}
