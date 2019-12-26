using System;

namespace CS.Edu.Core.Extensions
{
    public class GenericType
    {
        private readonly Type _type;

        public GenericType(Type type)
        {
            if(!(type.IsGenericType || type.IsGenericTypeDefinition))
                throw new InvalidOperationException("The definition needs to be a GenericType or GenericTypeDefinition");

            _type = type;
            GenericTypeDefinition = _type.GetGenericTypeDefinition();
            GenericParameterTypes = _type.GetGenericArguments();
        }

        public Type GenericTypeDefinition { get; }
        
        public Type[] GenericParameterTypes { get; }

        public static explicit operator GenericType(Type type) => new GenericType(type);
    }

    public static class TypeExt
    {
        public static readonly Type CLRRootType = typeof(object);
        
        public static bool IsSubclassOfExt(this object obj, Type baseType)
        {
            return obj.GetType().IsSubclassOfExt(baseType);
        }

        public static bool IsSubclassOfExt(this Type type, Type baseType)
        {
            while (type != null && type != CLRRootType)
            {
                if(type.IsGenericType)
                {
                    if(type.GetGenericTypeDefinition() == baseType)
                        return true;
                }

                type = type.BaseType;
            }

            return false;
        }
    }    
}