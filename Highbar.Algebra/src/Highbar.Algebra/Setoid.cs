namespace Highbar.Algebra
{
  public static class Setoid
  {
    public static SingleResult<X> Reflexive<X>(X value) => new SingleResult<X>(Laws.IsReflexive(value), value);

    public static DoubleResult<L,R> Symmetrical<L,R>(L left, R right) =>
      new DoubleResult<L,R>(Laws.IsSymmetrical(left, right), left, right);

    public static TripleResult<L,M,R> Transitive<L,M,R>(L left, M middle, R right) =>
      new TripleResult<L,M,R>(Laws.IsTransitive(left, middle, right), left, middle, right);

    private static class Laws
    {
      internal static bool IsReflexive(object value) => value.Equals(value);

      internal static bool IsSymmetrical(object left, object right) =>
        left.Equals(right) == right.Equals(left);

      internal static bool IsTransitive(object left, object middle, object right) =>
        (left.Equals(middle) && middle.Equals(right)) == left.Equals(right);
    }
  }
}
