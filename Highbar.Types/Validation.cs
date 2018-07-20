using System;
using System.Collections.Generic;
using System.Linq;

using static Highbar.Types.Objects;
using static Highbar.Functions.Functions;

namespace Highbar.Types
{
  public abstract class Validation<F, S>
  {
    /// <summary>
    /// Captures the possible <c>Exception</c> in a <c>Failure</c> otherwise the <c>value</c> in a <c>Success</c>.
    /// </summary>
    public static Validation<Exception, S> Attempt(Func<S> supplier)
    {
      try
      {
        return Pass<Exception, S>(supplier());
      } catch (Exception exception)
      {
        return Fail<Exception, S>(exception);
      }
    }

    /// <summary>
    /// Creates a <c>Failure</c> from an arbitrary value.
    /// </summary>
    public static Validation<SF, SS> Fail<SF, SS>(IEnumerable<SF> failures)
    {
      return new Failure<SF, SS>(failures);
    }

    /// <summary>
    /// Creates a <c>Failure</c> from an arbitrary value.
    /// </summary>
    public static Validation<SF, SS> Fail<SF, SS>(SF failure)
    {
      return new Failure<SF, SS>(new List<SF> { failure });
    }

    /// <summary>
    /// Creates a <c>Validation</c> from an arbitrary value.
    /// </summary>
    public static Validation<Exception, S> From(S value)
    {
      return OfNullable(value);
    }

    /// <summary>
    /// Creates a <c>Validation</c> from an <c>Either</c>.
    /// </summary>
    public static Validation<F, S> From(Either<F, S> either)
    {
      Validation<F, S> validation;

      if (either == null)
      {
        validation = Fail<F, S>(null);
      }
      else
      {
        validation = either.IsRight ? Pass<F, S>(either.GetRight()) : Fail<F, S>(either.GetLeft());
      }

      return validation;
    }

    /// <summary>
    /// Creates a <c>Validation</c> from the first element in the <c>list</c>.
    /// </summary>
    public static Validation<Exception, S> From(IEnumerable<S> values)
    {
      Validation<Exception, S> validation;

      if (values == null)
      {
        validation = Fail<Exception, S>(new NullReferenceException("list is null"));
      }
      else if (!values.Any())
      {
        validation = Fail<Exception, S>(new NullReferenceException("list is empty"));
      }
      else
      {
        validation = Pass<Exception, S>(values.First());
      }

      return validation;
    }

    /// <summary>
    /// Creates a <c>Validation</c> from a <c>Maybe</c>.
    /// </summary>
    public static Validation<Exception, S> From(Maybe<S> maybe)
    {
      if (maybe == null || maybe.IsNothing())
      {
        return Fail<Exception, S>(new NullReferenceException("value is null"));
      }
      else
      {
        return OfNullable(maybe.GetOrElse(default(S)));
      }
    }

    /// <summary>
    /// Creates an <c>Validation</c> from an <c>Validation</c>. Essentially identity function for <c>from</c>.
    /// </summary>
    public static Validation<F, S> From(Validation<F, S> validation)
    {
      return validation == null
        ? Fail<F, S>(null)
        : validation;
    }

    /// <summary>
    /// Returns the value within a <c>Failure</c>.
    /// </summary>
    public static IEnumerable<SF> FromFailure<SF, SS>(Validation<SF, SS> validation)
    {
      RequireNonNull(validation, "validation must not be null");

      if (validation.IsSuccess())
      {
        throw new ArgumentException("validation must not be a success");
      }

      return validation.GetFailures();
    }

    /// <summary>
    /// Returns the value with a <c>Success</c>.
    /// </summary>
    public static SS FromSuccess<SF, SS>(Validation<SF, SS> validation)
    {
      RequireNonNull(validation, "validation must not be null");

      if (validation.IsFailure())
      {
        throw new ArgumentException("validation must not be a failure");
      }

      return validation.GetSuccess();
    }

    /// <summary>
    /// Applicative <c>of</c>. Returns a <c>Validation</c> (read <c>Success</c>) of the value if the <c>value</c> is not <c>null</c>.
    /// </summary>
    public static Validation<F, S> Of(F failure, S value)
    {
      return value != null
        ? Pass<F, S>(value)
        : Fail<F, S>(failure);
    }

    /// <summary>
    /// Applicative <c>of</c>. Returns a <c>Validation</c> (read <c>Success</c>) of the value if the <c>value</c> is not <c>null</c>.
    /// </summary>
    public static Validation<F, S> Of(IEnumerable<F> failures, S value)
    {
      return value != null
        ? Pass<F, S>(value)
        : Fail<F, S>(failures);
    }

    /// <summary>
    /// Null sensitive <c>Success</c>-biased Applicative <c>of</c>.
    /// </summary>
    public static Validation<Exception, SS> OfNullable<SS>(SS value)
    {
      return value != null
        ? Pass<Exception, SS>(value)
        : Fail<Exception, SS>(new NullReferenceException("value is null"));
    }

    /// <summary>
    /// Creates a <c>Success</c> from an arbitrary value.
    /// </summary>
    public static Validation<SF, SS> Pass<SF, SS>(SS value)
    {
      return new Success<SF, SS>(value);
    }

    /// <summary>
    /// Alt <c>alt</c>. Replace a <c>Failure</c> with a <c>Success</c> of a value.
    /// </summary>
    public abstract Validation<F, S> Alt(Validation<F, S> other);

    /// <summary>
    /// Alt <c>alt</c>. Replace a <c>Failure</c> with a <c>Success</c> of a value produced by a <c>Supplier</c>.
    /// </summary>
    public abstract Validation<F, S> Alt(Func<Validation<F, S>> supplier);

    /// <summary>
    /// Apply <c>ap</c>. Applies the current <c>value</c> to the <c>value</c> of the <c>other</c>.
    /// </summary>
    public abstract Validation<F, R> Ap<R>(Validation<F, Func<S, R>> other);

    /// <summary>
    /// Bifunctor <c>bimap</c>. Maps either the <c>Failure</c> or <c>Success</c> value without needing to know the state of the <c>Validation</c>.
    /// </summary>
    public abstract Validation<G, T> Bimap<G, T>(
      Func<IEnumerable<F>, IEnumerable<G>> failureMap,
      Func<S, T> successMap
    );

    /// <summary>
    /// See Validation#Chain(Func) chain.
    /// </summary>
    public virtual Validation<F, R> Bind<R>(Func<S, Validation<F, R>> mapper) => Chain(mapper);

    /// <summary>
    /// Chain <c>chain</c> (a.k.a <c>bind</c>, <c>flatMap</c>). Takes a <c>Function</c> that accepts the <c>value</c> and returns a <c>Validation</c> of the return value.
    /// </summary>
    public abstract Validation<F, R> Chain<R>(Func<S, Validation<F, R>> mapper);

    /// <summary>
    /// See Validation#Alt(Validation) alt.
    /// </summary>
    public virtual Validation<F, S> Coalesce(Validation<F, S> other) => Alt(other);

    /// <summary>
    /// Semigroup <c>concat</c>. Concatenates the failures of the <c>instance</c> and <c>other</c> <c>Validation</c>s together.The intention of this function is to merge differing validations for the same underlying value. Two <c>Success</c> should reference the same object.
    /// Rules for concatenation:
    ///   - If <c>other</c> is <c>null</c>, return the <c>instance</c>.
    ///   - If <c>other</c> is equivalent to { @code instance }, return the <c>instance</c>.
    ///   - If both instances are a <c>Success</c>, return the <c>instance</c>.
    ///   - If both instances are a <c>Failure</c>, return a new <c>Failure</c> with the concatenated failure values.
    ///   - If <c>instance</c> is a <c>Failure</c>, return the <c>instance</c>.
    ///   - Otherwise, return the <c>other</c>.
    /// </summary>
    public virtual Validation<F, S> Concat(Validation<F, S> other)
    {
      Validation<F, S> result;

      if (other == null || this == other || (this.IsSuccess() && other.IsSuccess()))
      {
        result = this;
      }
      else if (this.IsFailure() && other.IsFailure())
      {
        result = Fail<F, S>(this.GetFailures().Concat(other.GetFailures()));
      }
      else if (this.IsFailure())
      {
        result = this;
      }
      else
      {
        result = other;
      }

      return result;
    }

    /// <summary>
    /// Extend <c>extend</c>. Takes a <c>Function</c> that accepts the <c>Validation</c> and returns a <c>value</c>.
    /// </summary>
    public abstract Validation<F, R> Extend<R>(Func<Validation<F, S>, R> mapper);

    /// <summary>
    /// See Validation#first(Function) first.
    /// </summary>
    public virtual Validation<G, S> FailureMap<G>(Func<IEnumerable<F>, IEnumerable<G>> mapper)
    {
      return First(mapper);
    }

    /// <summary>
    /// Applies the <c>predicate</c> to the <c>Success</c> value.
    /// </summary>
    public abstract Maybe<S> Filter(Predicate<S> predicate);

    /// <summary>
    /// Applies the <c>predicate</c> to the <c>Success</c> value.
    /// </summary>
    public abstract Validation<F, S> Filter(Predicate<S> predicate, F failureValue);

    /// <summary>
    /// Applies the <c>predicate</c> to the <c>Success</c> value.
    /// </summary>
    public abstract Validation<F, S> Filter(Predicate<S> predicate, Func<F> failureSupplier);

    /// <summary>
    /// Bifunctor <c>first</c>. Maps the <c>Failure</c> values if the <c>instance</c> is a <c>Failure</c>.
    /// </summary>
    public virtual Validation<G, S> First<G>(Func<IEnumerable<F>, IEnumerable<G>> mapper)
    {
      return Bimap(mapper, Identity<S>());
    }

    /// <summary>
    /// See Validation#Chain(Func) chain.
    /// </summary>
    public virtual Validation<F, T> FlatMap<T>(Func<S, Validation<F, T>> mapper)
    {
      return Chain(mapper);
    }

    /// <summary>
    /// Foldable <c>fold</c>. Folds the <c>Failure</c> value or the <c>Success</c> value into a singular unified type.
    /// </summary>
    public abstract T Fold<T>(Func<IEnumerable<F>, T> failureMapper, Func<S, T> successMapper);

    /// <summary>
    /// Gets the list of failures regardless of <c>Failure</c> or <c>Success</c> status.
    /// </summary>
    public virtual IEnumerable<F> GetFailures()
    {
      return null;
    }

    /// <summary>
    /// Returns the value if the <c>instance</c> is a <c>Success</c>, otherwise the <c>otherValue</c>.
    /// </summary>
    public abstract S GetOrElse(S otherValue);

    /// <summary>
    /// Returns the value if the <c>instance</c> is a <c>Success</c>, otherwise the failure-applied value of the <c>mapper</c>.
    /// </summary>
    public abstract S GetOrElseGet(Func<IEnumerable<F>, S> mapper);

    /// <summary>
    /// Returns the value if the <c>instance</c> is a <c>Success</c>, otherwise the value of the <c>supplier</c>.
    /// </summary>
    public abstract S GetOrElseGet(Func<S> supplier);

    /// <summary>
    /// Returns the value if the <c>instance</c> is a <c>Success</c>, otherwise throws the provided <c>Exception</c>.
    /// </summary>
    public abstract S GetOrElseThrow(Exception exception);

    /// <summary>
    /// Returns the value if the <c>instance</c> is a <c>Success</c>, otherwise throws an <c>Exception</c> mapped from the <c>Failure</c> values.
    /// </summary>
    public abstract S GetOrElseThrow(Func<IEnumerable<F>, Exception> failureMapper);

    /// <summary>
    /// Returns the value if the <c>instance</c> is a <c>Success</c>, otherwise throws the supplied <c>Throwable</c>.
    /// </summary>
    public abstract S GetOrElseThrow(Func<Exception> supplier);

    /// <summary>
    /// Gets the success value regardless of <c>Failure</c> or <c>Success</c> status.
    /// </summary>
    public virtual S GetSuccess()
    {
      return default(S);
    }

    /// <summary>
    /// Executes the <c>consumer</c> if the <c>instance</c> is a <c>Failure</c>.
    /// </summary>
    public abstract Validation<F, S> IfFailure(Action<IEnumerable<F>> consumer);

    /// <summary>
    /// Executes the <c>consumer</c> if the <c>instance</c> is a <c>Success</c>.
    /// </summary>
    public abstract Validation<F, S> IfSuccess(Action<S> consumer);

    /// <summary>
    /// Determines whether or not the <c>instance</c> is a <c>Failure</c>.
    /// </summary>
    public abstract bool IsFailure();

    /// <summary>
    /// Determines whether or not the <c>instance</c> is a <c>Success</c>.
    /// </summary>
    public abstract bool IsSuccess();

    /// <summary>
    /// Functor <c>map</c>. Takes a <c>Function</c> to map the biased right <c>value</c>.
    /// </summary>
    public abstract Validation<F, T> Map<T>(Func<S, T> mapper);

    /// <summary>
    /// Recover from a <c>Failure</c> into a possible <c>Success</c>.
    /// </summary>
    public abstract Validation<F, S> Recover(S value);

    /// <summary>
    /// Recover from a <c>Failure</c> into a <c>Success</c> with a value produced by a <c>Supplier</c>.
    /// </summary>
    public abstract Validation<F, S> Recover(Func<S> supplier);

    /// <summary>
    /// See Validation#Fold(Func, Func) fold.
    /// </summary>
    public virtual T Reduce<T>(Func<IEnumerable<F>, T> leftMapper, Func<S, T> rightMapper)
    {
      return Fold(leftMapper, rightMapper);
    }

    /// <summary>
    /// Bifunctor <c>second</c>.
    /// See Validation#Map(Func) map.
    /// </summary>
    public virtual Validation<F, T> Second<T>(Func<S, T> mapper)
    {
      return Bimap(Identity<IEnumerable<F>>(), mapper);
    }

    /// <summary>
    /// See Validation#Second(Func) second.
    /// </summary>
    public virtual Validation<F, T> SuccessMap<T>(Func<S, T> mapper)
    {
      return Second(mapper);
    }

    /// <summary>
    /// Taps into the underlying value of either the failure or success values.
    /// </summary>
    public virtual Validation<F, S> Tap(Action<IEnumerable<F>> failureConsumer, Action<S> successConsumer)
    {
      return IfFailure(RequireNonNull(failureConsumer))
        .IfSuccess(RequireNonNull(successConsumer));
    }

    /// <summary>
    /// Converts the <c>instance</c> to am <c>Either</c>.
    /// </summary>
    public abstract Either<F, S> ToEither();

    /// <summary>
    /// Converts the <c>instance</c> to a <c>List</c>.
    /// </summary>
    public abstract IList<S> ToList();

    /// <summary>
    /// Converts the <c>instance</c> to a <c>Maybe</c>.
    /// </summary>
    public abstract Maybe<S> ToMaybe();
  }
}
