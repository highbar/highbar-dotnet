using System;
using System.Collections.Generic;

using static Highbar.Types.Objects;

namespace Highbar.Types
{
  public class Right<L, R> : Either<L, R>
  {
    private R _value;

    public Right(R value)
    {
      _value = value;
    }

    public override bool IsLeft => false;

    public override bool IsRight => true;

    public override Either<L, R> Alt(Either<L, R> other)
    {
      return this;
    }

    public override Either<L, R> Alt(Func<Either<L, R>> supplier)
    {
      return this;
    }

    public override Either<L, R> Alt(Func<L, Either<L, R>> other)
    {
      return this;
    }

    public override Either<L, MR> Ap<MR>(Either<L, Func<R, MR>> other)
    {
      RequireNonNull(other, "other must not be null");

      return other.IsRight
        ? this.Map(other.GetRight())
        : new Left<L, MR>(other.GetLeft());
    }

    public override Either<ML, MR> Bimap<ML, MR>(Func<L, ML> leftMapper, Func<R, MR> rightMapper)
    {
      RequireNonNull(rightMapper, "rightMapper must not be null");

      return new Right<ML, MR>(rightMapper(GetRight()));
    }

    public override Either<L, MR> Chain<MR>(Func<R, Either<L, MR>> mapper)
    {
      RequireNonNull(mapper, "mapper must not be null");

      return mapper(GetRight());
    }

    private Either<L, Either<L, R>> Duplicate()
    {
      return new Right<L, Either<L, R>>(this);
    }

    public override bool Equals(object other)
    {
      return (other == this)
        || (other is Right<L, R> && GetRight().Equals(((Right<L, R>)other).GetRight()));
    }

    public override Either<L, MR> Extend<MR>(Func<Either<L, R>, MR> mapper)
    {
      RequireNonNull(mapper, "mapper must not be null");

      return Duplicate().Map(mapper);
    }

    public override Maybe<R> Filter(Predicate<R> predicate)
    {
      RequireNonNull(predicate, "predicate must not be null");

      return predicate(GetRight())
        ? Maybe<R>.Just(GetRight())
        : Maybe<R>.Nothing<R>();
    }

    public override Either<L, R> Filter(Predicate<R> predicate, L value)
    {
      RequireNonNull(predicate, "predicate must not be null");

      return predicate(GetRight())
        ? (Either<L, R>)this
        : new Left<L, R>(value);
    }

    public override Either<L, R> Filter(Predicate<R> predicate, Func<L> supplier)
    {
      RequireNonNull(predicate, "predicate must not be null");

      return predicate(GetRight())
        ? (Either<L, R>)this
        : new Left<L, R>(RequireNonNull(supplier, "supplier must not be null")());
    }

    public override V Fold<V>(Func<L, V> leftMapper, Func<R, V> rightMapper)
    {
      RequireNonNull(rightMapper, "rightMapper must not be null");

      return rightMapper(GetRight());
    }

    public override int GetHashCode()
    {
      return GetRight().GetHashCode();
    }

    public override L GetLeft()
    {
      return default(L);
    }

    public override R GetOrElse(R otherValue)
    {
      return GetRight();
    }

    public override R GetOrElseGet(Func<L, R> mapper)
    {
      return GetRight();
    }

    public override R GetOrElseGet(Func<R> supplier)
    {
      return GetRight();
    }

    public override R GetOrElseThrow(Func<L, Exception> mapper)
    {
      return GetRight();
    }

    public override R GetOrElseThrow(Func<Exception> supplier)
    {
      return GetRight();
    }

    public override R GetRight()
    {
      return _value;
    }

    public override Either<L, R> IfLeft(Action<L> consumer)
    {
      return this;
    }

    public override Either<L, R> IfRight(Action<R> consumer)
    {
      RequireNonNull(consumer, "consumer must not be null");

      consumer(GetRight());

      return this;
    }

    public override Either<L, MR> Map<MR>(Func<R, MR> mapper)
    {
      RequireNonNull(mapper, "mapper must not be null");

      return new Right<L, MR>(mapper(GetRight()));
    }

    public override Either<L, R> Recover(R value)
    {
      return this;
    }

    public override Either<L, R> Recover<A>(Func<A, R> other)
    {
      return this;
    }

    public override Either<L, R> Recover(Func<R> other)
    {
      return this;
    }

    public override IList<R> ToList()
    {
      return new List<R>() { GetRight() };
    }

    public override Maybe<R> ToMaybe()
    {
      return Maybe<R>.Just(GetRight());
    }

    public override string ToString()
    {
      return "Right{" + GetRight() + "}";
    }
  }
}
