using System.Collections;
using System.Collections.Generic;
using FD.Core.Editors;
using UnityEditor.Experimental.GraphView;
using System.Reflection;
using System;

public abstract class InventoryNodeRoot<T> : FAED_BaseNode where T : InventoryObjectRoot
{

    public event Action<UnityEngine.Object> OnSelectedEvent;

    private T invenObj;

    public InventoryNodeRoot(T invenObj) : base()
    {

        this.invenObj = invenObj;

        var method = typeof(T).GetMethod("GetSignal");
        var pram = method.GetParameters();

        var inputType = pram[0].GetCustomAttribute<BindParameterType>()?.bindType;
        var outputType = method.GetCustomAttribute<BindReturnType>()?.bindType;

        if (inputType == null) inputType = typeof(object);
        if (outputType == null) outputType = typeof(object);

        var inputPort = AddPort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, inputType);
        var outputPort = AddPort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, outputType);

        inputPort.title = $"input({inputType.Name})";
        outputPort.title = $"output({outputType.Name})";

    }

    public override void OnSelected()
    {

        base.OnSelected();

        OnSelectedEvent?.Invoke(invenObj);

    }

}