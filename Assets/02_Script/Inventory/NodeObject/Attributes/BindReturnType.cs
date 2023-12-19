using System;

[AttributeUsage(AttributeTargets.Method)]
public class BindReturnType : Attribute
{

    public Type bindType { get; private set; }

    public BindReturnType(Type bindType)
    {

        this.bindType = bindType;

    }

}