using System;
using System.Collections.Generic;

namespace Highbar.Types
{
  public class Nothing<V> : Maybe<V>
  {
    public override Maybe<V> Alt(Maybe<V> other)
    {
      Objects.RequireNonNull(other, "other must not be null");

      return other;
    }

    public override Maybe<V> Alt(Func<Maybe<V>> other)
    {
      Objects.RequireNonNull(other, "other must not be null");

      return other();
    }

    public override Maybe<R> Ap<R>(Maybe<Func<V, R>> other)
    {
      return Nothing<R>();
    }

    public override Maybe<R> Chain<R>(Func<V, Maybe<R>> mapper)
    {
      return Nothing<R>();
    }

    public override Maybe<R> CheckedMap<R>(Func<V, R> mapper)
    {
      return Nothing<R>();
    }

    public override Maybe<R> Extend<R>(Func<Maybe<V>, R> mapper)
    {
      return Nothing<R>();
    }

    public override Maybe<V> Filter(Predicate<V> predicate)
    {
      return this;
    }

    public override R FoldLeft<R>(Func<R, V, R> morphism, R initialValue)
    {
      Objects.RequireNonNull(morphism, "morphism must not be null");

      return morphism(initialValue, _value);
    }

    public override R FoldRight<R>(Func<V, R, R> morphism, R initialValue)
    {
      Objects.RequireNonNull(morphism, "morphism must not be null");

      return morphism(_value, initialValue);
    }

    public override V GetOrElse(V otherValue)
    {
      return otherValue;
    }

    public override V GetOrElseGet(Func<V> supplier)
    {
      return supplier();
    }

    public override V GetOrElseThrow(Func<Exception> supplier)
    {
      throw supplier();
    }

    public override Maybe<V> IfJust(Action<V> consumer)
    {
      return this;
    }

    public override Maybe<V> IfNothing(Action runnable)
    {
      runnable();

      return this;
    }

    public override bool IsJust()
    {
      return false;
    }

    public override Maybe<R> Map<R>(Func<V, R> mapper)
    {
      return Nothing<R>();
    }

    public override Maybe<V> Recover(V value)
    {
      Objects.RequireNonNull(value, "value must not be null");

      return Just(value);
    }

    public override Maybe<V> Recover(Func<V> supplier)
    {
      Objects.RequireNonNull(supplier, "supplier must not be null");

      return Just(supplier());
    }

    public override Either<Exception, V> ToEither()
    {
      return Either<Exception, V>.Left<Exception, V>(new NullReferenceException("value is null"));
    }

    public override IList<V> ToList()
    {
      return new List<V>();
    }

    public override int GetHashCode()
    {
      return 1;
    }

    public override bool Equals(object obj)
    {
      if(obj is Nothing<V>)
      {
        return true;
      }

      return false;
    }

    public override string ToString()
    {
      return "Nothing";
    }
  }
}
