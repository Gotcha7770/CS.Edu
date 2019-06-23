namespace CS.Edu.Core.Extensions
{
    public static class IntExt
    {
        public static bool IsEven(this int number)
        {
            return (number & 1) == 0;
        }
    }
}
