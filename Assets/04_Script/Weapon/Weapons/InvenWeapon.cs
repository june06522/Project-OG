using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InvenWeapon : Weapon
{
    protected Dictionary<int, SendData> sendDataList = new Dictionary<int, SendData>();
    public abstract void GetSignal(object signal);

    protected void LateUpdate()
    {
        Debug.Log("업데이트 돌고있음");
        foreach (var item in sendDataList)
        {
            SkillContainer.Instance.GetSKill((int)id, (int)item.Value.GeneratorID)?.Excute(transform, target, item.Value.Power);
        }
        sendDataList = new Dictionary<int, SendData>();

    }
}
