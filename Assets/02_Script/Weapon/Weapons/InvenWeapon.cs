using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InvenWeapon : Weapon
{
    protected Dictionary<GeneratorID, SendData> sendDataList = new Dictionary<GeneratorID, SendData>();
    public abstract void GetSignal(object signal);

    protected void LateUpdate()
    {
        foreach (var item in sendDataList)
        {
            SkillContainer.Instance.GetSKill((int)id, (int)item.Value.GeneratorID)?.Excute(transform, target, item.Value.Power);
        }
        sendDataList.Clear();

    }
}
