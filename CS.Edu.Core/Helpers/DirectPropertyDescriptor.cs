using System;
using System.ComponentModel;

namespace CS.Edu.Core.Helpers
{
    public class DirectPropertyDescriptor<TComponent, TProperty> : PropertyDescriptor
    {
        private static readonly Type _componentType;
        private static readonly Type _propertyType;

        private readonly Func<TComponent, TProperty> _getter;
        private readonly Action<TComponent, TProperty> _setter;

        static DirectPropertyDescriptor()
        {

            _componentType = typeof(TComponent);
            _propertyType = typeof(TProperty);
        }

        public DirectPropertyDescriptor(string name,
                                        Func<TComponent, TProperty> getter,
                                        Action<TComponent, TProperty> setter)
            : base(name, Array.Empty<Attribute>())
        {
            _getter = getter;
            _setter = setter;
        }

        public override Type ComponentType => _componentType;

        public override bool IsReadOnly => false;

        public override Type PropertyType => _propertyType;

        public override bool CanResetValue(object component) => false;

        public override object GetValue(object component)
        {
            return _getter((TComponent)component);
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            _setter((TComponent)component, (TProperty)value);
        }

        public override bool ShouldSerializeValue(object component) => false;
    }
}