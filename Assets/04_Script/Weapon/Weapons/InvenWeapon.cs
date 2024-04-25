using System.Collections.Generic;
using UnityEngine;

public abstract class InvenWeapon : Weapon
{
    [SerializeField] protected AudioClip _attackSoundClip;

    protected Dictionary<ulong, SendData> sendDataList = new Dictionary<ulong, SendData>();
    public abstract void GetSignal(object signal);

    protected void Update()
    {
        foreach (var item in sendDataList)
        {
            if(target != null)
            
            SkillContainer.Instance.GetSKill((int)id, (int)item.Value.GeneratorID)?.Excute(transform, target, item.Value.Power);
        }
        sendDataList = new Dictionary<ulong, SendData>();

    }
}
