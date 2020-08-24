using System;

namespace CS.Edu.Tests.Utils
{
    class TestClass : BaseNotifyPropertyChanged
    {
        private string _value;

        public TestClass() : this(string.Empty) 
        { }

        public TestClass(string value)
        {
            _value = value;
        }

        public string Value
        {
            get => _value;
            set => SetAndRaise(ref _value, value);
        }
    }
}