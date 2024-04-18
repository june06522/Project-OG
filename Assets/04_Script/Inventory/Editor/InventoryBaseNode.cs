using System.Collections;
using System.Collections.Generic;
using FD.Core.Editors;
using UnityEditor.Experimental.GraphView;
using System.Reflection;
using System;
using UnityEngine;
using UnityEditor;

public abstract class InventoryBaseNode : FAED_BaseNode
{

    public event Action<UnityEngine.Object> OnSelectedEvent;

    public InventoryObjectRoot invenObj { get; private set; }

    public InventoryBaseNode(Type invenType) : base()
    {

        invenObj = ScriptableObject.CreateInstance(invenType) as InventoryObjectRoot;
        invenObj.OnCreated();

    }

    public InventoryBaseNode(InventoryObjectRoot invenObj)
    {

        this.invenObj = invenObj;

    }

    public override void SetPosition(Rect newPos)
    {

        base.SetPosition(newPos);

        invenObj.editorPos.x = newPos.xMin;
        invenObj.editorPos.y = newPos.yMin;

        EditorUtility.SetDirty(invenObj);

    }
    public override void OnSelected()
    {

        base.OnSelected();

        OnSelectedEvent?.Invoke(invenObj);

    }

    public void Delete()
    {

        invenObj.DisconnectCall();
        GameObject.DestroyImmediate(invenObj);

    }

}