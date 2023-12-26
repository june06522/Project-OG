using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WeaponInvenConnectData
{

    public Vector2Int connectedPort;
    public InventoryObjectData connectedObject;

}

public class InvenBrick : MonoBehaviour
{

    private List<WeaponInvenConnectData> connectData = new();

    public InventoryObjectData InvenObject { get; private set; }
    public Vector2 InvenPoint { get; set; }

    protected void Awake()
    {

        InvenObject.Init(transform);

    }

    public void Setting(InventoryObjectData invenObject)
    {

        if(InvenObject != null)
        {

            InvenObject.OnSignalSend -= HandleSignalSend;

        }

        InvenObject = invenObject;

        InvenObject.OnSignalSend += HandleSignalSend;

    }


    private void HandleSignalSend(Vector2Int point, object signal)
    {

        var slot = connectData.Find(x => x.connectedPort == point);

        if (slot.connectedObject == null) return;

        slot.connectedObject.GetSignal(signal);

    }

}
