using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SynergeCardInfo")]
public class SynergyCardSO : ScriptableObject
{
    public string Name;
    public string Description;
    public TriggerID TriggerID;

    public string GetName(int count)
    {
        return $"{Name} {count}";
    }

    public string GetDescription(float percent)
    {
        //return $"{Description} {percent}%¡ı∞°";
        return $"{Description} {percent}%";
    }
}
