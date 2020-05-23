using System;

namespace CS.Edu.Tests.Utils
{
    class Selectable : BaseNotifyPropertyChanged
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetAndRaise(ref _isSelected, value);
        }

        public Guid Id { get; } = Guid.NewGuid();
    }
}