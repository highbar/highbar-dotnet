using System;
using System.Collections.Generic;

namespace Highbar.Types
{
  /// <summary>
  /// Wraps an arbitrary value in a <c>Right</c> or <c>Left</c>.  The <c>Either</c> is <c>Right</c> biased.  Typically,
  /// the <c>Either</c> represents a value from a successful operation or the error from a failed operation.
  /// </summary>
  public abstract class Either<L, R>
  {
    /// <summary>
    /// Captures the possible <c>Exception</c> in a <c>Left</c>, otherwise the <c>value</c> in a <c>Right</c>.
    /// </summary>
    public static Either<Exception, R> Attempt(Func<R> supplier)
    {
      try
      {
        return Right<Exception, R>(supplier());
      } 
      catch (Exception exception)
      {
        return Left<Exception, R>(exception);
      }
    }

    /// <summary>
    /// Creates an <c>Either</c> from an arbitrary value.
    /// </summary>
    public static Either<Exception, R> From(R right)
    {
      return OfNullable(right);
    }

    /// <summary>
    /// Creates an <c>Either</c> from an <c>Either</c>.  Essentially identity function for <c>from</c>.
    /// </summary>
    public static Either<L, R> From(Either<L, R> either)
    {
      return (either == null)
        ? Left<L, R>(default(L))
        : either;
    }

    /// <summary>
    /// Creates an <c>Either</c> from the first element in the <c>List</c>.
    /// </summary>
    public static Either<Exception, R> From(IList<R> list)
    {
      Either<Exception, R> result;

      if(list == null)
      {
        result = Left<Exception, R>(new NullReferenceException("list must not be null"));
      }
      else if(list.Count == 0)
      {
        result = Left<Exception, R>(new NullReferenceException("list must not be empty"));
      }
      else
      {
        result = OfNullable(list[0]);
      }

      return result;
    }

    /// <summary>
    /// Creates an <c>Either</c> from a <c>Maybe</c>.
    /// </summary>
    public static Either<Exception, SR> From<SR>(Maybe<SR> maybe)
    {
      return (maybe == null)
        ? Left<Exception, SR>(new NullReferenceException("maybe must not be null"))
        : OfNullable(maybe.GetOrElse(default(SR)));
    }

    /// <summary>
    /// Returns the value within a <c>Left</c>.
    /// </summary>
    public static SL FromLeft<SL, SR>(Either<SL, SR> either)
    {
      Objects.RequireNonNull(either, "either must not be null");

      if(either.IsRight)
      {
        throw new ArgumentException("either must not be a right");
      }

      return either.GetLeft();
    }

    /// <summary>
    /// Returns the value within a <c>Right</c>.
    /// </summary>
    public static SR FromRight<SL, SR>(Either<SL, SR> either)
    {
      Objects.RequireNonNull(either, "either must not be null");

      if(either.IsLeft)
      {
        throw new ArgumentException("either must not be a left");
      }

      return either.GetRight();
    }

    /// <summary>
    /// Creates a <c>Left</c> from an arbitrary value.
    /// </summary>
    public static Either<SL, SR> Left<SL, SR>(SL left)
    {
      return new Left<SL, SR>(left);
    }

    /// <summary>
    /// Applicative <c>of</c>.  Returns a <c>Either</c> (read: <c>Right</c>) of the value if <c>right</c> is not <c>null</c>.
    /// </summary>
    public static Either<SL, SR> Of<SL, SR>(SL left, SR right)
    {
      if(right == null)
      {
        return Left<SL, SR>(left);
      } 
      else
      {
        return Right<SL, SR>(right);
      }
    }

    /// <summary>
    /// Null-sensitive <c>Right</c>-biased Applicative <c>of</c>.
    /// </summary>
    public static Either<Exception, SR> OfNullable<SR>(SR value)
    {
      return (value == null)
        ? Left<Exception, SR>(new NullReferenceException("value must not be null"))
        : Right<Exception, SR>(value);
    }

    /// <summary>
    /// Creates a <c>Right</c> from an arbitrary value.
    /// </summary>
    public static Either<SL, SR> Right<SL, SR>(SR right)
    {
      return new Right<SL, SR>(right);
    }

    /// <summary>
    /// Alt <c>alt</c>.  Replace a <c>Left</c> with a <c>Right</c> of a value.
    /// </summary>
    public abstract Either<L, R> Alt(Either<L, R> other);

    /// <summary>
    /// Alt <c>alt</c>.  Replace a <c>Left</c> with a <c>Right</c> of a value produced by a <c>Func</c>.
    /// </summary>
    public abstract Either<L, R> Alt(Func<Either<L, R>> supplier);

    /// <summary>
    /// Alt <c>alt</c>.  Replace a <c>Left</c> with a <c>Right</c> of a value produced by a <c>Func</c>.
    /// </summary>
    public abstract Either<L, R> Alt(Func<L, Either<L, R>> other);

    /// <summary>
    /// Apply <c>ap</c>.  Applies the current <c>value</c> to the <c>value</c> of the <c>other</c>.
    /// </summary>
    public abstract Either<L, MR> Ap<MR>(Either<L, Func<R, MR>> other);

    /// <summary>
    /// Bifunctor <c>bimap</c>.  Maps either the <c>Left</c> or <c>Right</c> value without needing to know the state of the <c>Either</c>.
    /// </summary>
    public abstract Either<ML, MR> Bimap<ML, MR>(Func<L, ML> leftMapper, Func<R, MR> rightMapper);

    /// <summary>
    /// See Either#Chain(Func) chain.
    /// </summary>
    public Either<L, MR> Bind<MR>(Func<R, Either<L, MR>> mapper)
    {
      return Chain(mapper);
    }

    /// <summary>
    /// Chain <c>chain</c> (a.k.a. <c>bind</c> or <c>flatMap</c>).  Takes a <c>Func</c> that accepts the <c>value</c> and returns an <c>Either</c> of the return value.
    /// </summary>
    public abstract Either<L, MR> Chain<MR>(Func<R, Either<L, MR>> mapper);

    /// <summary>
    /// See Either#alt(Either) alt.
    /// </summary>
    public Either<L, R> Coalesce(Either<L, R> other)
    {
      return Alt(other);
    }

    /// <summary>
    /// Extend <c>extend</c>.  Takes a <c>Func</c> that accepts the <c>Either</c> and returns a <c>value</c>.
    /// </summary>
    public abstract Either<L, MR> Extend<MR>(Func<Either<L, R>, MR> mapper);

    /// <summary>
    /// Applies the <c>predicate</c> to the <c>Right</c> value.
    /// </summary>
    public abstract Maybe<R> Filter(Predicate<R> predicate);

    /// <summary>
    /// Applies the <c>predicate</c> to the <c>Right</c> value.
    /// </summary>
    public abstract Either<L, R> Filter(Predicate<R> predicate, L value);

    /// <summary>
    /// Applies the <c>predicate</c> to the <c>Right</c> value.
    /// </summary>
    public abstract Either<L, R> Filter(Predicate<R> predicate, Func<L> supplier);

    /// <summary>
    /// Bifunctor <c>first</c>.  Maps the <c>Left</c> value if the <c>instance</c> is a <c>Left</c>.
    /// </summary>
    public Either<ML, R> First<ML>(Func<L, ML> mapper)
    {
      return Bimap(mapper, i => i);
    }

    /// <summary>
    /// See Either#chain(Func) chain.
    /// </summary>
    public Either<L, MR> FlatMap<MR>(Func<R, Either<L, MR>> mapper)
    {
      return Chain(mapper);
    }

    /// <summary>
    /// Foldable <c>fold</c>.  Folds the <c>Left</c> value or the <c>Right</c> value into a singular unified type.
    /// </summary>
    public abstract V Fold<V>(Func<L, V> leftMapper, Func<R, V> rightMapper);

    /// <summary>
    /// Gets the left value regardless of <c>Left</c> or <c>Right</c> status.
    /// </summary>
    public abstract L GetLeft();

    /// <summary>
    /// Gets the right value regardless of <c>Left</c> or <c>Right</c> status.
    /// </summary>
    public abstract R GetRight();

    /// <summary>
    /// Returns the value if the <c>instance</c> is a <c>Right</c>, otherwise the <c>otherValue</c>.
    /// </summary>
    public abstract R GetOrElse(R otherValue);

    /// <summary>
    /// Returns the value if the <c>instance</c> is a <c>Right</c>, otherwise the left-applied value of the <c>mapper</c>.
    /// </summary>
    public abstract R GetOrElseGet(Func<L, R> mapper);

    /// <summary>
    /// Returns the value if the <c>instance</c> is a <c>Right</c>, otherwise the value of the <c>supplier</c>.
    /// </summary>
    public abstract R GetOrElseGet(Func<R> supplier);

    /// <summary>
    /// Returns the value if the <c>instance</c> is a <c>Right</c>, otherwise throws an <c>Exception</c> mapped from the <c>Left</c> value.
    /// </summary>
    public abstract R GetOrElseThrow(Func<L, Exception> mapper);

    /// <summary>
    /// Returns the value if the <c>instance</c> is a <c>Right</c>, otherwise throws the supplied <c>Exception</c>.
    /// </summary>
    public abstract R GetOrElseThrow(Func<Exception> supplier);

    /// <summary>
    /// Executes the <c>consumer</c> if the <c>instance</c> is a <c>Left</c>.
    /// </summary>
    public abstract Either<L, R> IfLeft(Action<L> consumer);

    /// <summary>
    /// Executes the <c>consumer</c> if the <c>instance</c> is a <c>Right</c>.
    /// </summary>
    public abstract Either<L, R> IfRight(Action<R> consumer);

    /// <summary>
    /// Determines whether or not the <c>instance</c> is a <c>Left</c>.
    /// </summary>
    public abstract bool IsLeft
    {
      get;
    }

    /// <summary>
    /// Determines whether or not the <c>instance</c> is a <c>Right</c>.
    /// </summary>
    public abstract bool IsRight
    {
      get;
    }

    /// <summary>
    /// See Either#first(Func) first.
    /// </summary>
    public Either<ML, R> LeftMap<ML>(Func<L, ML> mapper)
    {
      return First(mapper);
    }

    /// <summary>
    /// Functor <c>map</c>.  Takes a <c>Func</c> to map the biased right <c>value</c>.
    /// </summary>
    public abstract Either<L, MR> Map<MR>(Func<R, MR> mapper);

    /// <summary>
    /// Recover from a <c>Left</c> into a possible <c>Right</c>.
    /// </summary>
    public abstract Either<L, R> Recover(R value);

    /// <summary>
    /// Recover from a <c>Left</c> into a <c>Right</c> with a value produced by a <c>Func</c>.
    /// </summary>
    public abstract Either<L, R> Recover<A>(Func<A, R> other);

    /// <summary>
    /// Recover from a <c>Left</c> into a <c>Right</c> with a value produced by a <c>Func</c>.
    /// </summary>
    public abstract Either<L, R> Recover(Func<R> other);

    /// <summary>
    /// See Either#fold(Func, Func) fold.
    /// </summary>
    public V Reduce<V>(Func<L, V> leftMapper, Func<R, V> rightMapper)
    {
      return Fold(leftMapper, rightMapper);
    }

    /// <summary>
    /// See Either#second(Func) second.
    /// </summary>
    public Either<L, MR> RightMap<MR>(Func<R, MR> mapper)
    {
      return Second(mapper);
    }

    /// <summary>
    /// Bifunctor <c>second</c>.
    /// </summary>
    public Either<L, MR> Second<MR>(Func<R, MR> mapper)
    {
      return Bimap(i => i, mapper);
    }

    /// <summary>
    /// Taps into the underlying value of either the left or right values.
    /// </summary>
    public Either<L, R> Tap(Action<L> leftConsumer, Action<R> rightConsumer)
    {
      return IfLeft(leftConsumer).IfRight(rightConsumer);
    }

    /// <summary>
    /// Converts the <c>instance</c> to a <c>List</c>.
    /// </summary>
    public abstract IList<R> ToList();

    /// <summary>
    /// Converts the <c>instance</c> to a <c>Maybe</c>.
    /// </summary>
    public abstract Maybe<R> ToMaybe();
    }
}
