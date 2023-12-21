using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;

public class EventReveiveNode : InventoryBaseNode
{
    public EventReveiveNode(Type invenType) : base(invenType)
    {

        Create();

    }

    public EventReveiveNode(InventoryObjectRoot obj) : base(obj)
    {

        Create();

    }

    private void Create()
    {

        var method = invenObj.GetType().GetMethod("GetSignal");
        Type executeType = method.GetCustomAttribute<BindExecuteType>() == null ?
            typeof(object) : method.GetCustomAttribute<BindExecuteType>().bindType;


        var outputPort = AddPort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, executeType);
        outputPort.portName = $"output({executeType.Name})";

        title = invenObj.GetType().Name;

    }

}