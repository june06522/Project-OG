using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SignalPoint
{

    public Vector2Int point;
    public Vector2Int dir;

}

[Serializable]
public struct BrickPoint
{
    public Vector2Int point;
    public Vector2Int[] dir;
}

[CreateAssetMenu(menuName = "SO/Inventory/Object")]
public class InventoryObjectData : ScriptableObject
{

    [field:SerializeField] public List<BrickPoint> bricks { get; protected set; } = new();
    [field:SerializeField] public List<SignalPoint> inputPoints { get; protected set; } = new();
    [field:SerializeField] public List<SignalPoint> sendPoints { get; protected set; } = new();
    [HideInInspector] public List<InventoryObjectRoot> includes = new();

    private WeaponInventory inventory;

    public Action<object> OnSignalReceived;
    public Action<Vector2Int, object> OnSignalSend;

    public Vector2Int originPos { get; set; }

    public void Init(Transform owner)
    {

        inventory = UnityEngine.Object.FindObjectOfType<WeaponInventory>();

        for (int i = 0; i < includes.Count; i++)
        {

            includes[i] = includes[i].Copy();

        }

        for (int i = 0; i < includes.Count; i++)
        {

            includes[i].ResetConnect(includes);

        }

        for (int i = 0; i < includes.Count; i++)
        {

            includes[i].Init(owner, this);

        }

    }

    public void GetSignal(object signal)
    {

        OnSignalReceived?.Invoke(signal);

    }

    public void SendSignal(object signal)
    {

        List<InventoryObjectData> datas = new List<InventoryObjectData>();

        foreach (var item in sendPoints)
        {
            var d = inventory.GetObjectData(item.point, item.dir, originPos);

            if (d != null) datas.Add(d);

        }

        foreach (var item in datas)
        {

            item.GetSignal(signal);

        }

    }

}
