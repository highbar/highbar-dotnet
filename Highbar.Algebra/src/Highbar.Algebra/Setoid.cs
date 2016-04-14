namespace Highbar.Algebra
{
  public static class Setoid
  {
    public static Setoid<X,object,X> Reflexive<X>(X value) =>
      new Setoid<X,object,X>(Laws.IsReflexive(value), value, value, value);

    public static Setoid<L,object,R> Symmetrical<L,R>(L left, R right) =>
      new Setoid<L,object,R>(Laws.IsSymmetrical(left, right), left, null, right);

    public static Setoid<L,M,R> Transitive<L,M,R>(L left, M middle, R right) =>
      new Setoid<L,M,R>(Laws.IsTransitive(left, middle, right), left, middle, right);

    private static class Laws
    {
      internal static bool IsReflexive(object value) => value.Equals(value);

      internal static bool IsSymmetrical(object left, object right) =>
        left.Equals(right) == right.Equals(left);

      internal static bool IsTransitive(object left, object middle, object right) =>
        (left.Equals(middle) && middle.Equals(right)) == left.Equals(right);
    }
  }

  public class Setoid<L,M,R> : Algebra
  {
    public L Left { get; private set; }
    public M Middle { get; private set; }
    public R Right { get; private set; }

    internal Setoid(bool valid, L left, M middle, R right) : base(valid)
    {
      Left = left;
      Middle = middle;
      Right = right;
    }
  }
}
