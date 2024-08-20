using System;
using System.Linq;

namespace CS.Edu.Core.Extensions;

public class GenericType
{
    public GenericType(Type type)
    {
        if(!CanConvertFrom(type))
            throw new InvalidOperationException("The definition needs to be a GenericType or GenericTypeDefinition");

        GenericTypeDefinition = type.GetGenericTypeDefinition();
        GenericParameterTypes = type.GetGenericArguments();
    }

    public GenericType(Type type, Type[] parameterTypes)
    {
        if(!CanConvertFrom(type))
            throw new InvalidOperationException("The definition needs to be a GenericType or GenericTypeDefinition");

        if(parameterTypes is null || parameterTypes.IsEmpty())
            throw new InvalidOperationException("You should pass one or more generic parameter types");

        GenericTypeDefinition = type;
        GenericParameterTypes = parameterTypes;
    }

    public Type GenericTypeDefinition { get; }

    public Type[] GenericParameterTypes { get; }

    public static explicit operator GenericType(Type type) => new(type);

    public static bool CanConvertFrom(Type type)
    {
        return type != null && (type.IsGenericType || type.IsGenericTypeDefinition);
    }

    public static bool TryConvert(Type type, out GenericType result)
    {
        result = CanConvertFrom(type) ? (GenericType)type : null;

        return result != null;
    }
}

public static class Types
{
    public static readonly Type CLRRootType = typeof(object);

    public static bool IsSubclassOfGeneric(this object obj, Type baseType)
    {
        return obj.GetType().IsSubclassOfGeneric(baseType);
    }

    public static bool IsSubclassOfGeneric(this Type type, Type baseType) //???
    {
        return GenericType.CanConvertFrom(baseType)
               && type.IsSubclassOf((GenericType)baseType);
    }

    public static bool IsSubclassOf(this object obj, GenericType baseType)
    {
        return obj.GetType().IsSubclassOf(baseType);
    }

    public static bool IsSubclassOf(this Type type, GenericType baseType)
    {
        while (type != null && type != CLRRootType)
        {
            if(type.IsGenericType)
            {
                if(type.GetGenericTypeDefinition() == baseType.GenericTypeDefinition)
                    return true;
            }

            type = type.BaseType;
        }

        return false;
    }
}