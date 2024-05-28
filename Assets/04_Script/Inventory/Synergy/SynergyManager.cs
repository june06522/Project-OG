using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SynergyManager : MonoBehaviour
{

    private static SynergyManager instance;
    public static SynergyManager Instance => instance;


    
    Dictionary<TriggerID, int> synergyContainer;
    
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

        synergyContainer[id]++;

    }

    public void RemoveItem(TriggerID id)
    {

        if (synergyContainer.ContainsKey(id))
        {

            if (synergyContainer[id] != 0)
            {

                synergyContainer[id]--;

            }

        }

    }

    public void UpdateStat()
    {

    }
}