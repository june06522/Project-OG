using System;

[AttributeUsage(AttributeTargets.Method)]
public class BindReturnType : Attribute
{

    public Type returnType { get; private set; }

    public BindReturnType(Type returnType)
    {

        this.returnType = returnType;

    }

}