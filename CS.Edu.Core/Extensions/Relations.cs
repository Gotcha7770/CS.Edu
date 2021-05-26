namespace CS.Edu.Core.Extensions
{
    public static class Relations
    {
        public static Relation<bool> Or() => OrRelation.Value;

        public static Relation<bool> And() => AndRelation.Value;

        private static class OrRelation
        {
            internal static readonly Relation<bool> Value = Or;

            private static bool Or(bool first, bool second) => first || second;
        }

        private static class AndRelation
        {
            internal static readonly Relation<bool> Value = And;

            private static bool And(bool first, bool second) => first && second;
        }
    }
}