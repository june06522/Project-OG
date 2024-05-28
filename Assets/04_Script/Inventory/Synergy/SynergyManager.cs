using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SynergyManager : MonoBehaviour
{

    private static SynergyManager instance;
    public static SynergyManager Instance => instance;
    private void Awake()
    {

        if (instance != null)
        {

            Debug.LogError("Multiple SynergyManager is running");
            Destroy(instance);

        }

        instance = this;

    }

    public void AddItem()
    {

    }

    public void RemoveItem()
    {

    }

}
