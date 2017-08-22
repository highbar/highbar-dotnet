using System;
using System.Collections.Generic;

namespace Highbar.Types
{
  public class Left<L, R> : Either<L, R>
  {
    private L _value;

    public Left(L value)
    {
      _value = value;
    }

    public override bool IsLeft => true;

    public override bool IsRight => false;

    public override Either<L, R> Alt(Either<L, R> other)
    {
      return other;
    }

    public override Either<L, R> Alt(Func<Either<L, R>> supplier)
    {
      Objects.RequireNonNull(supplier, "supplier must not be null");

      return supplier();
    }

    public override Either<L, R> Alt(Func<L, Either<L, R>> other)
    {
      Objects.RequireNonNull(other, "other must not be null");

      return other(default(L));
    }

    public override Either<L, MR> Ap<MR>(Either<L, Func<R, MR>> other)
    {
      return other.IsLeft 
        ? new Left<L, MR>(Either<L, MR>.FromLeft(other))
        : new Left<L, MR>(Either<L, MR>.FromLeft(this));
    }

    public override Either<ML, MR> Bimap<ML, MR>(Func<L, ML> leftMapper, Func<R, MR> rightMapper)
    {
      Objects.RequireNonNull(leftMapper, "leftMapper must not be null");

      return new Left<ML, MR>(leftMapper(Either<ML, MR>.FromLeft(this)));
    }

    public override Either<L, MR> Chain<MR>(Func<R, Either<L, MR>> mapper)
    {
      return new Left<L, MR>(GetLeft());
    }

    public override bool Equals(object other)
    {
      return other == this
        || (other is Left<L, R> && EqualityComparer<L>.Default.Equals(GetLeft(), ((Left<L, R>)other).GetLeft()));
    }

    public override Either<L, MR> Extend<MR>(Func<Either<L, R>, MR> mapper)
    {
      return new Left<L, MR>(GetLeft());
    }

    public override Maybe<R> Filter(Predicate<R> predicate)
    {
      return Maybe<R>.Nothing<R>();
    }

    public override Either<L, R> Filter(Predicate<R> predicate, L value)
    {
       return new Left<L, R>(value);
    }

    public override Either<L, R> Filter(Predicate<R> predicate, Func<L> supplier)
    {
      return new Left<L, R>(GetLeft());
    }

    public override V Fold<V>(Func<L, V> leftMapper, Func<R, V> rightMapper)
    {
      Objects.RequireNonNull(leftMapper, "leftMapper must not be null");

      return leftMapper(GetLeft());
    }

    public override L GetLeft()
    {
      return _value;
    }

    public override R GetOrElse(R otherValue)
    {
      return otherValue;
    }

    public override R GetOrElseGet(Func<L, R> mapper)
    {
      Objects.RequireNonNull(mapper, "mapper must not be null");

      return mapper(GetLeft());
    }

    public override R GetOrElseGet(Func<R> mapper)
    {
      Objects.RequireNonNull(mapper, "mapper must not be null");

      return mapper();
    }

    public override R GetOrElseThrow(Func<L, Exception> mapper)
    {
      Objects.RequireNonNull(mapper, "mapper must not be null");

      throw mapper(GetLeft());
    }

    public override R GetOrElseThrow(Func<Exception> supplier)
    {
      Objects.RequireNonNull(supplier, "supplier must not be null");

      throw supplier();
    }

    public override R GetRight()
    {
      return default(R);
    }

    public override int GetHashCode()
    {
      return GetLeft().GetHashCode();
    }

    public override Either<L, R> IfLeft(Action<L> consumer)
    {
      Objects.RequireNonNull(consumer, "consumer must not be null");

      consumer(GetLeft());

      return this;
    }

    public override Either<L, R> IfRight(Action<R> consumer)
    {
      return this;
    }

    public override Either<L, MR> Map<MR>(Func<R, MR> mapper)
    {
      return new Left<L, MR>(GetLeft());
    }

    public override Either<L, R> Recover(R value)
    {
      Objects.RequireNonNull(value, "value must not be null");

      return new Right<L, R>(value);
    }

    public override Either<L, R> Recover<A>(Func<A, R> other)
    {
      Objects.RequireNonNull(other, "other must not be null");

      return new Right<L, R>(other(default(A)));
    }

    public override Either<L, R> Recover(Func<R> other)
    {
      Objects.RequireNonNull(other, "other must not be null");

      return new Right<L, R>(other());
    }

    public override IList<R> ToList()
    {
      return new List<R>();
    }

    public override Maybe<R> ToMaybe()
    {
      return Maybe<R>.Nothing<R>();
    }

    public override string ToString()
    {
      return "Left{" + GetLeft() + "}";
    }
  }
}
