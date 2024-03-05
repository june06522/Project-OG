using UnityEngine;

public class PlusOneConverter : InventoryConverterBase
{

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {

        Debug.Log("Connector");

        var sendData = (SendData)signal;
        sendData.Power++;

        foreach (var item in connectedOutput)
        {

            item.DoGetSignal(sendData);

        }

    }

}
