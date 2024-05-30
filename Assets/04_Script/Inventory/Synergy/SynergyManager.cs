using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SynergyData
{
    public TriggerID type;
    public List<float> table;
}
public class SynergyManager : MonoBehaviour
{

    private static SynergyManager instance;
    public static SynergyManager Instance => instance;

    Dictionary<TriggerID, int> synergyLevel;
    [SerializeField] List<SynergyData> tableList = new List<SynergyData>();

    private void Awake()
    {

        if (instance != null)
        {

            Debug.LogError("Multiple SynergyManager is running");
            Destroy(instance);

        }

        instance = this;

    }

    public void AddItem(TriggerID id)
    {

        synergyLevel[id]++;
        UpdateSynergy();
        float modifyAmount = tableList.FirstOrDefault((x) => x.type == id).table[synergyLevel[id]];

    }
     
    public void RemoveItem(TriggerID id)
    {

        if (synergyLevel.ContainsKey(id))
        {

            if (synergyLevel[id] != 0)
            {

                synergyLevel[id]--;

            }

        }

        UpdateSynergy();

    }

    public void UpdateSynergy()
    {

    }
}