using System.Collections;
using System.Collections.Generic;
using FD.Core.Editors;
using UnityEditor.Experimental.GraphView;
using System.Reflection;
using System;
using UnityEngine;

public abstract class InventoryBaseNode : FAED_BaseNode
{

    public event Action<UnityEngine.Object> OnSelectedEvent;

    protected InventoryObjectRoot invenObj;

    public InventoryBaseNode(Type invenType) : base()
    {

        invenObj = ScriptableObject.CreateInstance(invenType) as InventoryObjectRoot;

    }

    public override void OnSelected()
    {

        base.OnSelected();

        OnSelectedEvent?.Invoke(invenObj);


    }

}