using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ExecuterNode : InventoryBaseNode
{
    public ExecuterNode(Type invenType) : base(invenType)
    {

        Create();

    }

    public ExecuterNode(InventoryObjectRoot obj) : base(obj)
    {

        Create();

    }

    private void Create()
    {

        var method = invenObj.GetType().GetMethod("GetSignal");
        Type parmType = method.GetCustomAttribute<BindParameterType>() == null ?
            typeof(object) : method.GetCustomAttribute<BindParameterType>().bindType;


        var inputPort = AddPort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, parmType);
        inputPort.portName = $"output({parmType.Name})";

        title = invenObj.GetType().Name;

    }

}
