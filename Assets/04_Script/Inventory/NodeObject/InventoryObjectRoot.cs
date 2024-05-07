using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectType
{

    Input,
    Output,

}

public abstract class InventoryObjectRoot : ScriptableObject
{

    [HideInInspector] public string guid;

    protected Transform transform;
    protected GameObject gameObject;

    [HideInInspector] public List<InventoryObjectRoot> connectedInput = new(); //들어오는녀석
    [HideInInspector] public List<InventoryObjectRoot> connectedOutput = new(); //내보내는녀석
    protected InventoryObjectData data;
    public InventoryObjectData Data => data;

#if UNITY_EDITOR

     [HideInInspector] public Vector2 editorPos;

#endif

    public void OnCreated()
    {

        guid = Guid.NewGuid().ToString();

    }

    public void Connect(InventoryObjectRoot obj, ConnectType type)
    {

        if(type == ConnectType.Input)
        {

            if (connectedInput.Contains(obj)) return;

            connectedInput.Add(obj);

        }
        else
        {

            if (connectedOutput.Contains(obj)) return;

            connectedOutput.Add(obj);

        }

    }

    public void Disconnect(InventoryObjectRoot obj, ConnectType type)
    {

        if (type == ConnectType.Input)
        {
            if (!connectedInput.Contains(obj)) return;

            connectedInput.Remove(obj);

        }
        else
        {

            if (!connectedOutput.Contains(obj)) return;

            connectedOutput.Remove(obj);

        }

    }

    public void DisconnectCall()
    {

        foreach(var item in connectedInput)
        {

            item.Disconnect(this, ConnectType.Output);

        }

        foreach (var item in connectedOutput)
        {

            item.Disconnect(this, ConnectType.Input);

        }

    }

    public void Init(Transform owner, InventoryObjectData data)
    {

        transform = owner;
        gameObject = transform.gameObject;

        this.data = data;

        OnInit();

    }

    protected virtual void OnInit() { }

    public void DoGetSignal(object signal)
    {

        if(signal == null) return;

        GetSignal(signal);

    }

    public abstract void GetSignal(object signal);

    public virtual void ResetConnect(List<InventoryObjectRoot> copy)
    {

        for(int i = 0; i < connectedInput.Count; i++)
        {

            connectedInput[i] = copy.Find(x => x.guid == connectedInput[i].guid);

        }

        for (int i = 0; i < connectedOutput.Count; i++)
        {

            connectedOutput[i] = copy.Find(x => x.guid == connectedOutput[i].guid);

        }

    }

    public virtual InventoryObjectRoot Copy()
    {

        return Instantiate(this);

    }
}