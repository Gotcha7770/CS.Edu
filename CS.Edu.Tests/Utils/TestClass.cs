namespace CS.Edu.Tests.Utils
{
    class TestClass : BaseNotifyPropertyChanged
    {
        private string _value;

        public string Value
        {
            get => _value;
            set => SetAndRaise(ref _value, value);
        }
    }
}