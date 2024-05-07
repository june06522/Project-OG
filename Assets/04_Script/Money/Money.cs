using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoSingleton<Money>
{

    [field:SerializeField]
    public int Gold { get; private set; }
    public event Action<int> GoldChangedEvent;

    public void EarnGold(int gold)
    {
        Gold += gold;
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
}
