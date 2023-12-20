using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ConverterNode : InventoryBaseNode
{


    public ConverterNode(Type invenType) : base(invenType)
    {

        var method = invenType.GetMethod("GetSignal");
        Type executeType = method.GetCustomAttribute<BindExecuteType>() == null ?
            typeof(object) : method.GetCustomAttribute<BindExecuteType>().bindType;

        var parm = method.GetParameters();
        Type parmType = parm[0].GetCustomAttribute<BindParameterType>() == null ?
            typeof(object) : parm[0].GetCustomAttribute<BindParameterType>().bindType;

        var outputPort = AddPort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, executeType);
        outputPort.portName = $"output({executeType.Name})";

        var inputPort = AddPort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, parmType);
        inputPort.portName = $"input({parmType.Name})";

        title = invenType.Name;

    }

}
