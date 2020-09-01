using DynamicData;

namespace CS.Edu.Tests.Utils
{
    class Identity<T> : BaseNotifyPropertyChanged, IKey<T>
    {
        public Identity(T key)
        {
            Key = key;
        }
        public T Key { get; }
    }
}