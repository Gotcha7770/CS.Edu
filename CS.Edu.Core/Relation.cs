namespace CS.Edu.Core
{
    public delegate bool Relation<T>(T first, T second);

    public delegate bool Relation<in T1, in T2>(T1 first, T2 second);

    public delegate bool Relation<in T1, in T2, in T3>(T1 first, T2 second, T3 third);

    public delegate bool Relation<in T1, in T2, in T3, in T4>(T1 first, T2 second, T3 third, T4 forth);
}