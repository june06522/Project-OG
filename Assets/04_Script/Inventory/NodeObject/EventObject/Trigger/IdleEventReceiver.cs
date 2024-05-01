using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;
    public float cool;
    private float curCool;

    protected override void OnInit()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnIdle += ReductionVal;
            PlayerController.EventController.OnMove += ResetCool;
            curCool = cool;

        }

    }

    public void ReductionVal()
    {
        curCool -= Time.deltaTime;
        if (curCool < 0)
        {
            HandleDash();
        }
    }

    public void ResetCool()
    {
        curCool = cool;
    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void HandleDash()
    {

        SendData s = new SendData(generatorID, transform);

        GetSignal(s);

    }

    public override void Dispose()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnIdle -= ReductionVal;
            PlayerController.EventController.OnMove -= ResetCool;
        }

    }
}
