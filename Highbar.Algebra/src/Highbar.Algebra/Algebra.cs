namespace Highbar.Algebra
{
  public abstract class Algebra
  {
    protected Algebra() {}
    protected Algebra(bool valid)
    {
      Valid = valid;
    }

    public bool Valid { get; protected set; }

    protected T Perhaps<T>(T value)
    {
      return Valid ? value : default(T);
    }

    public static implicit operator bool(Algebra value)
    {
      return value.Valid;
    }
  }
}
