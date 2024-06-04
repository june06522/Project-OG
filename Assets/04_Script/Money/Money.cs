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
        //SynergyManager.Instance.OnSynergyChange += ChangeGoldFactor;
    }

    private void ChangeGoldFactor()
    {
        goldFactor = SynergyManager.Instance.SynergyAmount[PlayerStatsType.EarningGold];
    }

    public void EarnGold(int gold)
    {
        Gold += gold + Mathf.RoundToInt(gold * goldFactor);
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
        SynergyManager.Instance.OnSynergyChange -= ChangeGoldFactor;
    }
}
