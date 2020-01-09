using System;

namespace CS.Edu.Core.Extensions
{
    public class GenericType
    {
        private readonly Type _type;

        public GenericType(Type type)
        {
            if(!CanConvertFrom(type))
                throw new InvalidOperationException("The definition needs to be a GenericType or GenericTypeDefinition");

            _type = type;
            GenericTypeDefinition = _type.GetGenericTypeDefinition();
            GenericParameterTypes = _type.GetGenericArguments();
        }

        public Type GenericTypeDefinition { get; }
        
        public Type[] GenericParameterTypes { get; }

        public static explicit operator GenericType(Type type) => new GenericType(type);

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

    public static class TypeExt
    {
        public static readonly Type CLRRootType = typeof(object);
        
        public static bool IsSubclassOfGeneric(this object obj, Type baseType)
        {
            return obj.GetType().IsSubclassOfGeneric(baseType);
        }

        public static bool IsSubclassOfGeneric(this Type type, Type baseType) //???
        {
            return GenericType.CanConvertFrom(baseType) 
                ? type.IsSubclassOf((GenericType)baseType)
                : false;
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
}