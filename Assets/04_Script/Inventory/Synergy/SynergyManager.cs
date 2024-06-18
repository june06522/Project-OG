using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SynergyManager : MonoSingleton<SynergyManager>
{
    public Dictionary<TriggerID, int> level = new Dictionary<TriggerID, int>();
    [SerializeField] SerializableDictionary<TriggerID, List<float>> table = new SerializableDictionary<TriggerID, List<float>>();

    public Action OnSynergyChange;

    private void Awake()
    {
        table.GetContainer();
        level.Clear();

        foreach (TriggerID item in Enum.GetValues(typeof(TriggerID)))
        {
            level.Add(item, 0);
        }
    }

    public void EquipItem(TriggerID id)
    {
        if (id == TriggerID.Regist)
            id = TriggerID.Move;
        if (id == TriggerID.RoomEnter)
            id = TriggerID.StageClear;

        if (level.ContainsKey(id))
        {

            level[id]++;

        }
        else
        {

            level.Add(id, 1);

        }

        OnSynergyChange?.Invoke();

    }

    public void RemoveItem(TriggerID id)
    {
        if (id == TriggerID.Regist)
            id = TriggerID.Move;
        if (id == TriggerID.RoomEnter)
            id = TriggerID.StageClear;

        level[id]--;
        OnSynergyChange?.Invoke();

    }

    public float GetStatFactor(TriggerID id)
    {
        if (table.dict.ContainsKey(id) && level.ContainsKey(id))
        {
            if (level[id] <= 9)
                return table.dict[id][level[id]];
            else
                return table.dict[id][9];
        }
        else return 0;
    }

}

[Serializable]
public class SerializableDictionary<T1, T2>
{
    public List<SerializeData<T1, T2>> data;
    public Dictionary<T1, T2> dict = new Dictionary<T1, T2>();

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