using UnityEngine;

public class PlusOneConverter : InventoryConverterBase
{

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {
        var sendData = (SendData)signal;
        
        sendData.Power++;

        foreach (var item in connectedOutput)
        {
            if (sendData.isVisited.ContainsKey(item))
                continue;
            sendData.isVisited[item] = true;
            item.DoGetSignal(sendData);

        }

    }

}
