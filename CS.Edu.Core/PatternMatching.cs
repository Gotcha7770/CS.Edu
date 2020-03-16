namespace CS.Edu.Core
{
    public static class PatternMatching
    {
        public static int Add(int? x, int? y) => (x, y) switch
        {
            (int a, int b) => a + b,
            (int a, _) => a,
            (_, int b) => b,
            _ => 0
        };
    }
}
