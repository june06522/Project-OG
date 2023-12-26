using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SignalPoint
{

    public Vector2Int point;
    public Vector2Int dir;

}

[CreateAssetMenu(menuName = "SO/Inventory/Object")]
public class InventoryObjectData : ScriptableObject
{

    public List<Vector2Int> bricks { get; protected set; } = new();
    public List<SignalPoint> signalPoints { get; protected set; } = new();
    [HideInInspector] public List<InventoryObjectRoot> includes = new();

    public Action<object> OnSignalReceived;
    public Action<Vector2Int, object> OnSignalSend;

    public void Init(Transform owner)
    {

        for(int i  = 0; i < includes.Count; i++)
        {

            includes[i] = includes[i].Copy();

        }

        for (int i = 0; i < includes.Count; i++)
        {

            includes[i].ResetConnect(includes);

        }

        for (int i = 0; i < includes.Count; i++)
        {

            includes[i].Init(owner);

        }

    }

    public void GetSignal(object signal)
    {

        OnSignalReceived?.Invoke(signal);

    }

    public void SendSignal(Vector2Int point, object signal)
    {

        if (!bricks.Contains(point)) return;

        OnSignalSend?.Invoke(point, signal);

    }

}
