using System;
using System.Collections.Generic;

namespace Highbar.Types
{
  public abstract class Maybe<V>
  {
    protected V _value;

    /// <summary>
    /// Returns <c>Nothing</c> if evaluating <c>supplier</c> throws an Exception.
    /// </summary>
    public static Maybe<V> Attempt(Func<V> supplier)
    {
      try
      {
        return Just(supplier());
      }
      catch(Exception)
      {
        return Nothing<V>();
      }
    }

    /// <summary>
    /// Alternative <c>empty</c>.
    /// </summary>
    public static Maybe<R> Empty<R>()
    {
      return Nothing<R>();
    }

    /// <summary>
    /// Creates a <c>Maybe</c> from an arbitrary value.
    /// </summary>
    public static Maybe<R> From<R>(R value)
    {
      return (value == null)
        ? Nothing<R>()
        : Just(value);
    }

    /// <summary>
    /// Creates a <c>Maybe</c> from an arbitrary nullable value type.
    /// </summary>
    public static Maybe<R> From<R>(Nullable<R> value) where R : struct
    {
      return (value.HasValue)
        ? Just(value.Value)
        : Nothing<R>();
    }

    /// <summary>
    /// Creates a <c>Maybe</c> from a <c>Maybe</c>.  Essentially an identity function for <c>From</c>.
    /// </summary>
    public static Maybe<R> From<R>(Maybe<R> maybe)
    {
      return maybe ?? Nothing<R>();
    }

    /// <summary>
    /// Creates a <c>Maybe</c> from the first element in a <c>List</c>.
    /// </summary>
    public static Maybe<R> From<R>(IList<R> list)
    {
      return (list == null || list.Count <= 0)
        ? Nothing<R>()
        : OfNullable(list[0]);
    }

    /// <summary>
    /// Returns the value inside the <c>Maybe</c>.
    /// </summary>
    public static R FromJust<R>(Maybe<R> maybe)
    {
      Objects.RequireNonNull(maybe, "maybe must not be null");

      if(maybe.IsNothing())
      {
        throw new ArgumentException("maybe must not be Nothing");
      }

      return maybe._value;
    }

    /// <summary>
    /// Creates a <c>Just</c> from an arbitrary value.
    /// </summary>
    public static Maybe<R> Just<R>(R value)
    {
      Objects.RequireNonNull(value, "value must not be null");

      return new Just<R>(value);
    }

    /// <summary>
    /// Returns a <c>Func</c> that takes a <c>Maybe</c> and returns the <c>defaultValue</c> for a <c>Nothing</c> or
    /// the mapped result of the <c>mapper</c>.
    /// </summary>
    public static Func<Maybe<T>, R> MaybeMap<T, R>(R defaultValue, Func<T, R> mapper)
    {
      return instance => MaybeMap<T, R>(defaultValue, mapper, instance);
    }

    /// <summary>
    /// Returns the <c>defaultValue</c> if the <c>Maybe</c> is null or <c>Nothing</c>, otherwise the result of
    /// evaluating the <c>mapper</c>.
    /// </summary>
    public static R MaybeMap<T, R>(R defaultValue, Func<T, R> mapper, Maybe<T> maybe)
    {
      if(maybe != null && maybe.IsJust())
      {
        return mapper(maybe._value);
      }
      else
      {
        return defaultValue;
      }
    }

    /// <summary>
    /// Returns a <c>Nothing</c>.
    /// </summary>
    public static Maybe<R> Nothing<R>()
    {
      return new Nothing<R>();
    }

    /// <summary>
    /// Creates a <c>Just</c> of an arbitrary value.
    /// </summary>
    public static Maybe<R> Of<R>(R value)
    {
      return Just(value);
    }

    /// <summary>
    /// Creates a <c>Just</c> of an arbitrary value, or a <c>Nothing</c> if the value is null.
    /// </summary>
    public static Maybe<R> OfNullable<R>(R value)
    {
      return (value == null)
        ? Nothing<R>()
        : Just(value);
    }

    /// <summary>
    /// Alternative <c>alt</c>.  Replaces a <c>Nothing</c> with a <c>Maybe</c> of a value.
    /// </summary>
    public abstract Maybe<V> Alt(Maybe<V> other);

    /// <summary>
    /// Alternative <c>alt</c>.  Replaces a <c>Nothing</c> with a <c>Maybe</c> produced by a supplier.
    /// </summary>
    public abstract Maybe<V> Alt(Func<Maybe<V>> other);

    /// <summary>
    /// Apply <c>ap</c>.  Applies the current value to the value of the other.
    /// </summary>
    public abstract Maybe<R> Ap<R>(Maybe<Func<V, R>> other);

    /// <summary>
    /// See Maybe#Chain.
    /// </summary>
    public Maybe<R> Bind<R>(Func<V, Maybe<R>> mapper)
    {
      return Chain<R>(mapper);
    }

    /// <summary>
    /// Chain <c>chain</c> (aka bind or flatMap).  Takes a <c>Func</c> that accepts the current <c>value</c> and
    /// returns a <c>Maybe</c> of the return value.
    /// </summary>
    public abstract Maybe<R> Chain<R>(Func<V, Maybe<R>> mapper);

    /// <summary>
    /// Wraps execution of the <c>mapper</c> in a try...catch where successful mapping is returned as a <c>Just</c>
    /// or the caught exception is ignored and a <c>Nothing</c> is returned.
    /// </summary>
    public abstract Maybe<R> CheckedMap<R>(Func<V, R> mapper);

    /// <summary>
    /// See Maybe#Alt.
    /// </summary>
    public Maybe<V> Coalesce(Maybe<V> other)
    {
      return Alt(other);
    }

    /// <summary>
    /// Extend <c>extend</c>.  Takes a <c>Func</c> that takes a <c>Maybe</c> and returns an unboxed value.
    /// </summary>
    public abstract Maybe<R> Extend<R>(Func<Maybe<V>, R> mapper);

    /// <summary>
    /// Applies the <c>predicate</c> to the current <c>value</c>.
    /// </summary>
    public abstract Maybe<V> Filter(Predicate<V> predicate);

    /// <summary>
    /// See Maybe#Chain.
    /// </summary>
    public Maybe<R> FlatMap<R>(Func<V, Maybe<R>> mapper)
    {
      return Chain<R>(mapper);
    }

    /// <summary>
    /// Foldable <c>foldLeft</c>.  Left-associative fold into a summary value.
    /// </summary>
    public abstract R FoldLeft<R>(Func<R, V, R> morphism, R initialValue);

    /// <summary>
    /// Foldable <c>foldRight</c>.  Right-associative fold into a summary value.
    /// </summary>
    public abstract R FoldRight<R>(Func<V, R, R> morphism, R initialValue);

    /// <summary>
    /// Returns the value if the current instance is a <c>Just</c>, otherwise the otherValue.
    /// </summary>
    public abstract V GetOrElse(V otherValue);

    /// <summary>
    /// Returns the value if the current instance is a <c>Just</c>, otherwise runs the <c>supplier</c> and returns the
    /// result.
    /// </summary>
    public abstract V GetOrElseGet(Func<V> supplier);

    /// <summary>
    /// Returns the value if the current instance is a <c>Just</c>, otherwise runs the <c>supplier</c> and throws the
    /// resuling exception.
    /// </summary>
    public abstract V GetOrElseThrow(Func<Exception> supplier);

    /// <summary>
    /// Executes the <c>consumer</c> if the current instance is a <c>Just</c>.
    /// </summary>
    public abstract Maybe<V> IfJust(Action<V> consumer);

    /// <summary>
    /// Executes the <c>runnable</c> if the current instance is a <c>Nothing</c>.
    /// </summary>
    public abstract Maybe<V> IfNothing(Action runnable);

    /// <summary>
    /// Determines whether or not the current instance is a <c>Just</c>.
    /// </summary>
    public abstract bool IsJust();

    /// <summary>
    /// Determines whether or not the current instance is a <c>Nothing</c>.
    /// </summary>
    public bool IsNothing()
    {
      return !IsJust();
    }

    /// <summary>
    /// Functor <c>map</c>.  Takes a <c>Func</c> that maps the value.
    /// </summary>
    public abstract Maybe<R> Map<R>(Func<V, R> mapper);

    /// <summary>
    /// Recover from a <c>Nothing</c> into a possible <c>Just</c>.
    /// </summary>
    public abstract Maybe<V> Recover(V value);

    /// <summary>
    /// Recover from a <c>Nothing</c> into a possible <c>Just</c>.
    /// </summary>
    public abstract Maybe<V> Recover(Func<V> supplier);

    /// <summary>
    /// Taps into the underlying value of the <c>Maybe</c>.
    /// </summary>
    public Maybe<V> Tap(Action runnable, Action<V> consumer)
    {
      Objects.RequireNonNull(runnable, "runnable must not be null");
      Objects.RequireNonNull(consumer, "consumer must not be null");

      return IfNothing(runnable)
        .IfJust(consumer);
    }

    // <summary>
    /// Converts the instance to a <c>List</c>.
    /// </summary>
    public abstract IList<V> ToList();
  }
}
