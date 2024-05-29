using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SynergyManager : MonoBehaviour
{

    private static SynergyManager instance;
    public static SynergyManager Instance => instance;
    
    Dictionary<TriggerID, int> synergyCount;
    
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

        synergyCount[id]++;
        UpdateSynergy();

    }

    public void RemoveItem(TriggerID id)
    {

        if (synergyCount.ContainsKey(id))
        {

            if (synergyCount[id] != 0)
            {

                synergyCount[id]--;

            }

        }

        UpdateSynergy();

    }

    public void UpdateSynergy()
    {

    }
}