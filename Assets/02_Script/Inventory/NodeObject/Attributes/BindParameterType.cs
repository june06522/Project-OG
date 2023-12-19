using System;

[AttributeUsage(AttributeTargets.Parameter)]
public class BindParameterType : Attribute
{

    public Type bindType { get; private set; }

    public BindParameterType(Type bindType)
    {

        this.bindType = bindType;

    }

}