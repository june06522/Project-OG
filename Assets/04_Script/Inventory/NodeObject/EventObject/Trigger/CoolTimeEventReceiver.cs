using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTimeEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;
    public float cool;
    private float curCool;

    protected override void OnInit()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnCool += ReductionVal;
            curCool = cool;

        }

    }

    public void ReductionVal()
    {
        curCool -= Time.deltaTime;
        if(curCool < 0)
        {
            curCool = cool;
            HandleCool();
        }
    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void HandleCool()
    {

        SendData s = new SendData(generatorID);

        GetSignal(s);

    }

    public override void Dispose()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnCool -= ReductionVal;

        }

    }
}
