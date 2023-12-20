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

        var method = invenType.GetMethod("GetSignal");
        Type executeType = method.GetCustomAttribute<BindExecuteType>() == null ? 
            typeof(object) : method.GetCustomAttribute<BindExecuteType>().bindType;


        var outputPort = AddPort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, executeType);
        outputPort.portName = $"output({executeType.Name})";

        title = invenType.Name;

    }

}