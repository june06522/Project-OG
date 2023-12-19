using System;

[AttributeUsage(AttributeTargets.Parameter)]
public class BindParameterType : Attribute
{

    public Type returnType { get; private set; }

    public BindParameterType(Type returnType)
    {

        this.returnType = returnType;

    }

}