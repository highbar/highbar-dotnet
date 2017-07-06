using System;
using System.Collections.Generic;

namespace Highbar.Types
{
  public class Just<V> : Maybe<V>
  {
    public Just(V value)
    {
      Objects.RequireNonNull(value, "value must not be null");

      _value = value;
    }

    public override Maybe<V> Alt(Maybe<V> other)
    {
      Objects.RequireNonNull(other, "other must not be null");

      return this;
    }

    public override Maybe<V> Alt(Func<Maybe<V>> other)
    {
      Objects.RequireNonNull(other, "other must not be null");

      return this;
    }

    public override Maybe<R> Ap<R>(Maybe<Func<V, R>> other)
    {
      Objects.RequireNonNull(other, "other must not be null");

      Maybe<Maybe<R>> maybe = other.Map(Map);

      return maybe.IsJust()
        ? FromJust(maybe)
        : Nothing<R>();
    }

    public override Maybe<R> Chain<R>(Func<V, Maybe<R>> mapper)
    {
      Objects.RequireNonNull(mapper, "mapper must not be null");

      Maybe<Maybe<R>> maybe = Map(mapper);

      return maybe.IsJust()
        ? FromJust(maybe)
        : Nothing<R>();
    }

    public override Maybe<R> CheckedMap<R>(Func<V, R> mapper)
    {
      try
      {
        return OfNullable(mapper(_value));
      }
      catch
      {
        return Nothing<R>();
      }
    }

    public override Maybe<R> Extend<R>(Func<Maybe<V>, R> mapper)
    {
      Objects.RequireNonNull(mapper, "mapper must not be null");

      return Just(this).Map(mapper);
    }

    public override Maybe<V> Filter(Predicate<V> predicate)
    {
      Objects.RequireNonNull(predicate, "predicate must not be null");

      return predicate(_value)
        ? this
        : Nothing<V>();
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
      return _value;
    }

    public override V GetOrElseGet(Func<V> supplier)
    {
      Objects.RequireNonNull(supplier, "supplier must not be null");

      return _value;
    }

    public override V GetOrElseThrow(Func<Exception> supplier)
    {
      Objects.RequireNonNull(supplier, "supplier must not be null");

      return _value;
    }

    public override Maybe<V> IfJust(Action<V> consumer)
    {
      Objects.RequireNonNull(consumer, "consumer must not be null");

      consumer(_value);

      return this;
    }

    public override Maybe<V> IfNothing(Action runnable)
    {
      Objects.RequireNonNull(runnable, "runnable must not be null");

      return this;
    }

    public override bool IsJust()
    {
      return true;
    }

    public override Maybe<R> Map<R>(Func<V, R> mapper)
    {
      Objects.RequireNonNull(mapper, "mapper must not be null");

      return OfNullable(mapper(_value));
    }

    public override Maybe<V> Recover(V value)
    {
      Objects.RequireNonNull(value, "value must not be null");

      return this;
    }

    public override Maybe<V> Recover(Func<V> supplier)
    {
      Objects.RequireNonNull(supplier, "supplier must not be null");

      return this;
    }

    public override IList<V> ToList()
    {
      return new List<V> { _value };
    }

    public override int GetHashCode()
    {
      return _value.GetHashCode();
    }

    public override bool Equals(object o)
    {
      Just<V> other = null;

      if(o is Just<V>)
      {
        other = (Just<V>)o;
      }

      return other != null && other._value.Equals(this._value);
    }

    public override string ToString()
    {
      return "Just{" + _value + "}";
    }
  }
}
