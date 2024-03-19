using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InvenWeapon : Weapon
{
    protected SendData sendDatas;

    public abstract void GetSignal(object signal);

    protected void LateUpdate()
    {
        if (sendDatas != null)
        {
            Debug.Log(sendDatas.Power);
            SkillContainer.Instance.GetSKill((int)id, (int)sendDatas.GeneratorID)?.Excute(transform, target, sendDatas.Power);
            sendDatas = null;
        }

    }
}
