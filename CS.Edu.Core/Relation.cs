namespace CS.Edu.Core;

public delegate bool Relation<in T>(T one, T other);

public delegate bool Relation<in T1, in T2>(T1 one, T2 other);

public delegate bool Relation<in T1, in T2, in T3>(T1 first, T2 second, T3 third);

public delegate bool Relation<in T1, in T2, in T3, in T4>(T1 first, T2 second, T3 third, T4 forth);