using System;

namespace CS.Edu.Tests.Utils
{
    class Selectable<T> : Valuable<T>
    {
        private bool _isSelected;

        public Selectable(Guid key, T value)
            : base(key, value)
        { }

        public Selectable(T value)
            : base(value)
        { }

        public Selectable()
            : base(default)
        { }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetAndRaise(ref _isSelected, value);
        }
    }
}