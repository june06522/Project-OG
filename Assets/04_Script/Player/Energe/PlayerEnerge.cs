using System;
using UnityEngine;

public class PlayerEnerge : MonoBehaviour
{
    // maxEnerge, currentEnerge
    public event Action<int, int> OnChangeEnergeEvent;

    public int MaxEnerge { get; private set; }
    public int CurrentEnerge { get; private set; }

    public float RegenEnergePerSec { get; private set; }
    private float _regenEnerge;

    private void Update()
    {
        RegenEnerge();
    }

    private void RegenEnerge()
    {
        if (CurrentEnerge >= MaxEnerge) return;

        _regenEnerge += Time.deltaTime * RegenEnergePerSec;
        if(_regenEnerge > 1)
        {
            _regenEnerge--;
            RestoreEnerge(1);
        }
    }
    public void RestoreEnerge(int restoreEnerge)
    {
        CurrentEnerge = Math.Clamp(CurrentEnerge + restoreEnerge, 0, MaxEnerge);
        OnChangeEnergeEvent?.Invoke(MaxEnerge, CurrentEnerge);
    }
    public bool ConsumeEnerge(int minus)
    {
        if(CurrentEnerge < minus)
            return false;

        CurrentEnerge -= minus;
        OnChangeEnergeEvent?.Invoke(MaxEnerge, CurrentEnerge);
        return true;
    }
    public void SetPlayerEnerge(int maxEnerge, float regenEnergePerSec)
    {
        MaxEnerge = maxEnerge;
        CurrentEnerge = maxEnerge;
        OnChangeEnergeEvent?.Invoke(MaxEnerge, CurrentEnerge);

        RegenEnergePerSec = regenEnergePerSec;
    }
}
