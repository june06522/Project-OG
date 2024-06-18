using System;
using UnityEngine;

public class Money : MonoSingleton<Money>
{

    [field: SerializeField]
    public int Gold { get; private set; }
    public event Action<int> GoldChangedEvent;

    private float goldFactor;

    private void Start()
    {
        SynergyManager.Instance.OnSynergyChange += ChangeGoldFactor;
    }

    private void ChangeGoldFactor()
    {

        // 둘이 똑같은거임
        goldFactor = SynergyManager.Instance.GetStatFactor(TriggerID.StageClear);
        //goldFactor = SynergyManager.Instance.GetStatFactor(TriggerID.RoomEnter);

    }

    public void EarnGold(int gold, bool force = false)
    {
        if (force)
        {
            Gold += gold;
            GoldChangedEvent?.Invoke(Gold);

            return;
        }

        Gold += gold + Mathf.CeilToInt(gold * goldFactor);
        GoldChangedEvent?.Invoke(Gold);
    }

    public bool SpendGold(int gold)
    {
        if (Gold < gold)
            return false;

        Gold -= gold;
        GoldChangedEvent?.Invoke(Gold);
        return true;
    }

    private void OnDestroy()
    {
        if (SynergyManager.Instance != null)
            SynergyManager.Instance.OnSynergyChange -= ChangeGoldFactor;
    }
}
