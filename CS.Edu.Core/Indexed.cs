namespace CS.Edu.Core
{
    public struct Indexed<T>
    {
        public Indexed(int index, T value)
        {
            Index = index;
            Value = value;
        }

        public int Index { get; }

        public T Value { get; }
    }
}