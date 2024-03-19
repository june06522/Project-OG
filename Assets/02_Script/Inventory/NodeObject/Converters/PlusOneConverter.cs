using System.Collections;
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
            if (sendData.isVisited.ContainsKey(item.Data.originPos) && sendData.isVisited[item.Data.originPos] > sendData.Power)
                continue;

            if (sendData.isVisited.ContainsKey(item.Data.originPos))
                continue;

            sendData.isVisited[item.Data.originPos] = sendData.Power;
            item.DoGetSignal(sendData);

        }

    }

}
