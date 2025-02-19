using System.Collections;
using UnityEngine;

public class PlusOneConverter : InventoryConverterBase
{

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {
        var sendData = (SendData)signal;


        foreach (var item in connectedOutput)
        {
            //if (sendData.isVisited.ContainsKey(item.Data.originPos) && sendData.isVisited[item.Data.originPos] > sendData.Power)
            //    continue;

            if (sendData.checkVisit.ContainsKey(item.Data.originPos))
                continue;

            SendData tempdata = new(sendData.GeneratorID, transform,sendData.TriggerID,sendData.targetCnt,sendData.Power,sendData.startWeapon);
            tempdata.index = sendData.index;

            tempdata.checkVisit = new();

            foreach (DictionaryEntry item2 in sendData.checkVisit)
            {
                tempdata.checkVisit.Add(item2.Key, item2.Value);
            }

            tempdata.Power++;
            tempdata.checkVisit.Add(item.Data.originPos, 1);
            tempdata.isVisited[item.Data.originPos] = tempdata.Power;
            item.DoGetSignal(tempdata);


            //sendData.checkVisit.Remove(item.Data.originPos);
            //sendData.Power--;

        }

    }

}
