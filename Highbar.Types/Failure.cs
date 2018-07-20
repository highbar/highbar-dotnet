using System;
using System.Collections.Generic;
using System.Linq;

using static Highbar.Types.Objects;

namespace Highbar.Types
{
  public class Failure<F, S> : Validation<F, S>
  {
    private readonly IEnumerable<F> _failures;

    public Failure(IEnumerable<F> failures)
    {
      _failures = failures;
    }

    public override Validation<F, S> Alt(Validation<F, S> other)
    {
      return other;
    }

    public override Validation<F, S> Alt(Func<Validation<F, S>> supplier)
    {
      return RequireNonNull(supplier)();
    }

    public override Validation<F, R> Ap<R>(Validation<F, Func<S, R>> other)
    {
      return other.IsFailure()
        ? Fail<F, R>(FromFailure(other))
        : Fail<F, R>(this.GetFailures());
    }

    public override Validation<G, T> Bimap<G, T>(Func<IEnumerable<F>, IEnumerable<G>> failureMap, Func<S, T> successMap)
    {
      return Fail<G, T>(RequireNonNull(failureMap)(this.GetFailures()));
    }

    public override Validation<F, R> Chain<R>(Func<S, Validation<F, R>> mapper)
    {
      return Fail<F, R>(this.GetFailures());
    }

    public override bool Equals(object other)
    {
      return other == this
        || (other is Validation<F, S> && Enumerable.SequenceEqual<F>(this.GetFailures(), ((Validation<F, S>)other).GetFailures()));
    }

    public override Validation<F, R> Extend<R>(Func<Validation<F, S>, R> mapper)
    {
      return Fail<F, R>(GetFailures());
    }

    public override Maybe<S> Filter(Predicate<S> predicate)
    {
      return Maybe<S>.Nothing<S>();
    }

    public override Validation<F, S> Filter(Predicate<S> predicate, F failureValue)
    {
      return this;
    }

    public override Validation<F, S> Filter(Predicate<S> predicate, Func<F> failureSupplier)
    {
      return this;
    }

    public override T Fold<T>(Func<IEnumerable<F>, T> failureMapper, Func<S, T> successMapper)
    {
      return RequireNonNull(failureMapper)(GetFailures());
    }

    public override IEnumerable<F> GetFailures()
    {
      return _failures;
    }

    public override int GetHashCode()
    {
      return GetFailures().GetHashCode();
    }

    public override S GetOrElse(S otherValue)
    {
      return otherValue;
    }

    public override S GetOrElseGet(Func<IEnumerable<F>, S> mapper)
    {
      return RequireNonNull(mapper)(GetFailures());
    }

    public override S GetOrElseGet(Func<S> supplier)
    {
      return RequireNonNull(supplier)();
    }

    public override S GetOrElseThrow(Exception exception)
    {
      throw RequireNonNull(exception);
    }

    public override S GetOrElseThrow(Func<IEnumerable<F>, Exception> failureMapper)
    {
      throw RequireNonNull(failureMapper)(GetFailures());
    }

    public override S GetOrElseThrow(Func<Exception> supplier)
    {
      throw RequireNonNull(supplier)();
    }

    public override Validation<F, S> IfFailure(Action<IEnumerable<F>> consumer)
    {
      RequireNonNull(consumer)(GetFailures());

      return this;
    }

    public override Validation<F, S> IfSuccess(Action<S> consumer)
    {
      return this;
    }

    public override bool IsFailure()
    {
      return true;
    }

    public override bool IsSuccess()
    {
      return false;
    }

    public override Validation<F, T> Map<T>(Func<S, T> mapper)
    {
      return Fail<F, T>(GetFailures());
    }

    public override Validation<F, S> Recover(S value)
    {
      return Pass<F, S>(RequireNonNull(value));
    }

    public override Validation<F, S> Recover(Func<S> supplier)
    {
      return Pass<F, S>(RequireNonNull(supplier)());
    }

    public override Either<F, S> ToEither()
    {
      return Either<F, S>.Left<F, S>(GetFailures().FirstOrDefault(IsNotNull));
    }

    public override IList<S> ToList()
    {
      return new List<S>();
    }

    public override Maybe<S> ToMaybe()
    {
      return Maybe<S>.Nothing<S>();
    }
  }
}
