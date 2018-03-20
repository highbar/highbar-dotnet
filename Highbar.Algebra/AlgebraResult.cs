namespace Highbar.Algebra
{
  public abstract class AlgebraResult
  {
    protected AlgebraResult() {}
    protected AlgebraResult(bool valid)
    {
      Valid = valid;
    }

    public bool Valid { get; protected set; }

    protected T Perhaps<T>(T value)
    {
      return Valid ? value : default(T);
    }

    public static implicit operator bool(AlgebraResult value)
    {
      return value.Valid;
    }
  }
}
