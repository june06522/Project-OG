using System;
using System.Collections.Generic;
using UnityEngine;

public class SynergyManager : MonoSingleton<SynergyManager>
{
    public Dictionary<TriggerID, int> level;
    [SerializeField] SerializableDictionary<TriggerID, List<float>> table;

    public Action OnSynergyChange;

    public void EquipItem(TriggerID id)
    {

        level[id]++;
        OnSynergyChange?.Invoke();

    }

    public void RemoveItem(TriggerID id)
    {

        level[id]--;
        OnSynergyChange?.Invoke();

    }

    public float GetStatFactor(TriggerID id)
    {
        return table.GetContainer()[id][level[id]];
    }

}

[Serializable]
public class SerializableDictionary<T1, T2>
{
    public List<SerializeData<T1, T2>> data;
    private Dictionary<T1, T2> dict = new Dictionary<T1, T2>();

    public Dictionary<T1, T2> GetContainer()
    {
        for (int i = 0; i < data.Count; i++)
        {
            dict.Add(data[i].key, data[i].Value);
        }
        return dict;
    }

}

[Serializable]
public class SerializeData<T1, T2>
{
    public T1 key;
    public T2 Value;
}