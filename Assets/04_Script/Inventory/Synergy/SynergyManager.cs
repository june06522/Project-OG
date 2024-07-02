using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SynergyManager : MonoSingleton<SynergyManager>
{
    public Dictionary<TriggerID, int> level = new Dictionary<TriggerID, int>();
    [SerializeField] SerializableDictionary<TriggerID, List<float>> table = new SerializableDictionary<TriggerID, List<float>>();
    [SerializeField] Dictionary<TriggerID, List<int>> levelTable = new();
    public Action OnSynergyChange;

    private void Awake()
    {
        table.GetContainer();
        level.Clear();
        levelTable.Clear();

        foreach (TriggerID item in Enum.GetValues(typeof(TriggerID)))
        {
            level.Add(item, 0);
        }
        InitLevelTable();
    }

    private void InitLevelTable()
    {
        levelTable.Add(TriggerID.Dash, new() { 3, 5, 7 });
        levelTable.Add(TriggerID.NormalAttack, new() { 3, 5, 7, 9 });
        levelTable.Add(TriggerID.Move, new() { 2, 4, 6, 8 });
        levelTable.Add(TriggerID.CoolTime, new() { 3, 6, 9 });
        levelTable.Add(TriggerID.Idle, new() { 3, 5, 7, 9});
        levelTable.Add(TriggerID.RoomEnter, new() { 1, 2, 3, 4, 5});
        levelTable.Add(TriggerID.StageClear, new() { 1, 2, 3, 4, 5 });
        levelTable.Add(TriggerID.GetHit, new() { 2, 4, 6, 8 });
        levelTable.Add(TriggerID.Kill, new() { 2, 4, 6, 8 });
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

    public List<int> GetLevelTable(TriggerID id)
    {
        if (levelTable.ContainsKey(id))
            return levelTable[id];

        return new List<int>();
    }

    public int GetSynergyLevel(TriggerID id)
    {
        if(level.ContainsKey(id))
            return level[id];   

        return 0;
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