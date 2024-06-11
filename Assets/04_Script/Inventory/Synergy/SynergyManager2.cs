using System;
using System.Collections.Generic;
using UnityEngine;

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

public class SynergyManager2 : MonoSingleton<SynergyManager2>
{
    Dictionary<TriggerID, int> level;
    [SerializeField] SerializableDictionary<TriggerID, List<float>> table;
    public void EquipItem(TriggerID id) => level[id]++;
    public void RemoveItem(TriggerID id) => level[id]--;

    public float GetStatFactor(TriggerID id)
    {
        return table.GetContainer()[id][level[id]];
    }

}
