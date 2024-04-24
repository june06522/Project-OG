using System.Collections.Generic;

public abstract class InvenWeapon : Weapon
{
    protected Dictionary<ulong, SendData> sendDataList = new Dictionary<ulong, SendData>();
    public abstract void GetSignal(object signal);

    protected void Update()
    {
        foreach (var item in sendDataList)
        {
            SkillContainer.Instance.GetSKill((int)id, (int)item.Value.GeneratorID)?.Excute(transform, target, item.Value.Power);
        }
        sendDataList = new Dictionary<ulong, SendData>();

    }
}
