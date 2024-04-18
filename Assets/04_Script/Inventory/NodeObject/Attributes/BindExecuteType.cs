using System;

[AttributeUsage(AttributeTargets.Method)]
public class BindExecuteType : Attribute
{

    public Type bindType { get; private set; }

    public BindExecuteType(Type bindType)
    {

        this.bindType = bindType;

    }

}