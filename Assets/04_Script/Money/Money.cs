using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public static Money Instance { get; private set; }

    [field:SerializeField]
    public int Gold { get; private set; }
    public event Action<int> GoldChangedEvent;

    private void Awake()
    {
        #region 초기화
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"{transform} : Money is multiply running!");
            Destroy(this);
        }
        #endregion
    }

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
