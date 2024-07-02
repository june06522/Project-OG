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
    public List<SignalPoint> inputPoints { get; protected set; } = new();
    public List<SignalPoint> sendPoints { get; protected set; } = new();
    private HashSet<Vector2Int> pointdata = new();
    [HideInInspector] public List<InventoryObjectRoot> includes = new();

    public GeneratorID generatorID = GeneratorID.None;

    private WeaponInventory inventory;

    public Action<object> OnSignalReceived;
    public Action<Vector2Int, object> OnSignalSend;

    public Vector2Int originPos { get; set; }
    public Material colorMat;

    [HideInInspector] public InvenBrick invenBrick;

    public void Init(Transform owner)
    {
        FillPoints();

        inventory = FindObjectOfType<WeaponInventory>();

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

        invenBrick = owner.GetComponent<InvenBrick>();
    }

    private void FillPoints()
    {
        //brick point information
        foreach (BrickPoint brick in bricks)
            pointdata.Add(brick.point);

        //send point
        foreach(BrickPoint brick in bricks)
        {
            foreach(Vector2Int vec in brick.dir)
            {
                if(!pointdata.Contains(vec + brick.point))
                {
                    SignalPoint brickpoint = new();
                    brickpoint.point = brick.point;
                    brickpoint.dir = vec;
                    sendPoints.Add(brickpoint);
                }
            }
        }

        //input point
        foreach(var sendPoint in sendPoints)
        {
            SignalPoint brickpoint = new();
            brickpoint.point = sendPoint.point;
            brickpoint.dir = sendPoint.dir * -1;
            inputPoints.Add(brickpoint);
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
