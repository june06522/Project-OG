using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEventReceiver : InventoryEventReceiverBase
{
    public int chargeValue;
    public WeaponType targetType;
    public GeneratorID generatorID;

    [SerializeField] SendDataSO sendData;
    
    protected override void OnInit()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnDash += HandleDash;

        }

    }

    [BindExecuteType(typeof(SendDataSO))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void HandleDash()
    {
        
        GetSignal(sendData);

    }

    public override void Dispose()
    {

        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnDash -= HandleDash;

        }

    }

}
