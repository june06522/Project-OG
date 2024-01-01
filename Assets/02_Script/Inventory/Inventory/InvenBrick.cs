using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InvenBrick : MonoBehaviour
{

    [field:SerializeField] public InventoryObjectData InvenObject { get; private set; }
    public Vector2 InvenPoint { get; set; }

    protected virtual void Awake()
    {

        InvenObject = Instantiate(InvenObject);
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

        

    }

}
