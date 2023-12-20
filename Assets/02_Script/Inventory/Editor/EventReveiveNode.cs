using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using System;

public class EventReveiveNode : InventoryBaseNode
{
    public EventReveiveNode(Type invenType) : base(invenType)
    {

        var method = invenType.GetMethod("GetSignal");
        Debug.Log(method.Name);
        var executeType = method.GetCustomAttribute<BindExecuteType>().bindType;

        var outputPort = AddPort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, executeType);
        outputPort.title = $"output({executeType.Name})";

    }

}