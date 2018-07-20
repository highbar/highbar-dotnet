using System;
using System.Collections.Generic;
using System.Linq;

using static Highbar.Types.Objects;

namespace Highbar.Types
{
  public class Success<F, S> : Validation<F, S>
  {
    private S _value;

    public Success(S value)
    {
      _value = value;
    }

    public override Validation<F, S> Alt(Validation<F, S> other)
    {
      return this;
    }

    public override Validation<F, S> Alt(Func<Validation<F, S>> supplier)
    {
      return this;
    }

    public override Validation<F, R> Ap<R>(Validation<F, Func<S, R>> other)
    {
      RequireNonNull(other);

      return other.IsSuccess()
        ? Map(FromSuccess(other))
        : Fail<F, R>(FromFailure(other));
    }

    public override Validation<G, T> Bimap<G, T>(Func<IEnumerable<F>, IEnumerable<G>> failureMap, Func<S, T> successMap)
    {
      return Pass<G, T>(RequireNonNull(successMap)(GetSuccess()));
    }

    public override Validation<F, R> Chain<R>(Func<S, Validation<F, R>> mapper)
    {
      return RequireNonNull(mapper)(GetSuccess());
    }

    public override bool Equals(object other)
    {
      return other == this
        || (other is Validation<F, S> && EqualityComparer<S>.Default.Equals(this.GetSuccess(), ((Validation<F, S>)other).GetSuccess()));
    }

    public override Validation<F, R> Extend<R>(Func<Validation<F, S>, R> mapper)
    {
      return Duplicate().Map(RequireNonNull(mapper));
    }

    public override Maybe<S> Filter(Predicate<S> predicate)
    {
      return RequireNonNull(predicate)(GetSuccess())
        ? Maybe<S>.Just(GetSuccess())
        : Maybe<S>.Nothing<S>();
    }

    public override Validation<F, S> Filter(Predicate<S> predicate, F failureValue)
    {
      return RequireNonNull(predicate)(GetSuccess())
        ? this
        : Fail<F, S>(failureValue);
    }

    public override Validation<F, S> Filter(Predicate<S> predicate, Func<F> failureSupplier)
    {
      return RequireNonNull(predicate)(GetSuccess())
        ? this
        : Fail<F, S>(RequireNonNull(failureSupplier)());
    }

    public override T Fold<T>(Func<IEnumerable<F>, T> failureMapper, Func<S, T> successMapper)
    {
      return RequireNonNull(successMapper)(GetSuccess());
    }

    public override int GetHashCode()
    {
      return GetSuccess().GetHashCode();
    }

    public override S GetOrElse(S otherValue)
    {
      return GetSuccess();
    }

    public override S GetOrElseGet(Func<IEnumerable<F>, S> mapper)
    {
      return GetSuccess();
    }

    public override S GetOrElseGet(Func<S> supplier)
    {
      return GetSuccess();
    }

    public override S GetOrElseThrow(Exception exception)
    {
      return GetSuccess();
    }

    public override S GetOrElseThrow(Func<IEnumerable<F>, Exception> failureMapper)
    {
      return GetSuccess();
    }

    public override S GetOrElseThrow(Func<Exception> supplier)
    {
      return GetSuccess();
    }

    public override S GetSuccess()
    {
      return _value;
    }

    public override Validation<F, S> IfFailure(Action<IEnumerable<F>> consumer)
    {
      return this;
    }

    public override Validation<F, S> IfSuccess(Action<S> consumer)
    {
      RequireNonNull(consumer)(GetSuccess());

      return this;
    }

    public override bool IsFailure()
    {
      return false;
    }

    public override bool IsSuccess()
    {
      return true;
    }

    public override Validation<F, T> Map<T>(Func<S, T> mapper)
    {
      return Pass<F, T>(RequireNonNull(mapper)(GetSuccess()));
    }

    public override Validation<F, S> Recover(S value)
    {
      return this;
    }

    public override Validation<F, S> Recover(Func<S> supplier)
    {
      return this;
    }

    public override Either<F, S> ToEither()
    {
      return Either<F, S>.Right<F, S>(GetSuccess());
    }

    public override IList<S> ToList()
    {
      return new List<S> { GetSuccess() };
    }

    public override Maybe<S> ToMaybe()
    {
      return Maybe<S>.Just(GetSuccess());
    }

    private Validation<F, Validation<F, S>> Duplicate()
    {
      return Pass<F, Validation<F, S>>(this);
    }
  }
}
