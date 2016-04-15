namespace Highbar.Algebra
{
  public class SingleResult<X> : AlgebraResult
  {
    public X Value { get; private set; }

    internal SingleResult(bool valid, X value) : base(valid)
    {
      Value = value;
    }
  }

  public class DoubleResult<L, R> : AlgebraResult
  {
    public L Left { get; private set; }
    public R Right { get; private set; }

    internal DoubleResult(bool valid, L left, R right) : base(valid)
    {
      Left = left;
      Right = right;
    }
  }

  public class TripleResult<L, M, R> : AlgebraResult
  {
    public L Left { get; private set; }
    public M Middle { get; private set; }
    public R Right { get; private set; }

    internal TripleResult(bool valid, L left, M middle, R right) : base(valid)
    {
      Left = left;
      Middle = middle;
      Right = right;
    }
  }
}
