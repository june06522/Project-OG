using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InvenWeapon : Weapon
{
    protected Dictionary<SendData, int> sendDataList = new Dictionary<SendData, int>();
    public abstract void GetSignal(object signal);

    protected void LateUpdate()
    {
        foreach (var item in sendDataList)
        {
            SkillContainer.Instance.GetSKill((int)id, (int)item.Key.GeneratorID)?.Excute(transform, target, item.Value);
        }
        sendDataList = new Dictionary<SendData, int>();

    }
}
