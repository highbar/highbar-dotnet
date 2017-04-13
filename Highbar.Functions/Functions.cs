using System;

namespace Highbar.Functions
{
  public static class Functions
  {
    public static Func<A, B> Always<A, B>(B alwaysValue)
    {
      return ignoredA => alwaysValue;
    }

    public static Func<A, C> Pipe<A, B, C>(Func<A, B> first, Func<B, C> second)
    {
      return value => second(first(value));
    }

    public static Func<A, D> Pipe<A, B, C, D>(
      Func<A, B> first,
      Func<B, C> second,
      Func<C, D> third)
    {
      return value => third(second(first(value)));
    }

    public static Func<A, E> Pipe<A, B, C, D, E>(
      Func<A, B> first,
      Func<B, C> second,
      Func<C, D> third,
      Func<D, E> fourth)
    {
      return value => fourth(third(second(first(value))));
    }

    public static Func<A, F> Pipe<A, B, C, D, E, F>(
      Func<A, B> first,
      Func<B, C> second,
      Func<C, D> third,
      Func<D, E> fourth,
      Func<E, F> fifth)
    {
      return value => fifth(fourth(third(second(first(value)))));
    }

    public static Func<A, G> Pipe<A, B, C, D, E, F, G>(
      Func<A, B> first,
      Func<B, C> second,
      Func<C, D> third,
      Func<D, E> fourth,
      Func<E, F> fifth,
      Func<F, G> sixth)
    {
      return value => sixth(fifth(fourth(third(second(first(value))))));
    }

    public static Func<A, H> Pipe<A, B, C, D, E, F, G, H>(
      Func<A, B> first,
      Func<B, C> second,
      Func<C, D> third,
      Func<D, E> fourth,
      Func<E, F> fifth,
      Func<F, G> sixth,
      Func<G, H> seventh)
    {
      return value => seventh(sixth(fifth(fourth(third(second(first(value)))))));
    }

    public static Func<A, I> Pipe<A, B, C, D, E, F, G, H, I>(
      Func<A, B> first,
      Func<B, C> second,
      Func<C, D> third,
      Func<D, E> fourth,
      Func<E, F> fifth,
      Func<F, G> sixth,
      Func<G, H> seventh,
      Func<H, I> eighth)
    {
      return value => eighth(seventh(sixth(fifth(fourth(third(second(first(value))))))));
    }

    public static Func<A, J> Pipe<A, B, C, D, E, F, G, H, I, J>(
      Func<A, B> first,
      Func<B, C> second,
      Func<C, D> third,
      Func<D, E> fourth,
      Func<E, F> fifth,
      Func<F, G> sixth,
      Func<G, H> seventh,
      Func<H, I> eighth,
      Func<I, J> ninth)
    {
      return value => ninth(eighth(seventh(sixth(fifth(fourth(third(second(first(value)))))))));
    }

     public static Func<A, K> Pipe<A, B, C, D, E, F, G, H, I, J, K>(
      Func<A, B> first,
      Func<B, C> second,
      Func<C, D> third,
      Func<D, E> fourth,
      Func<E, F> fifth,
      Func<F, G> sixth,
      Func<G, H> seventh,
      Func<H, I> eighth,
      Func<I, J> ninth,
      Func<J, K> tenth)
    {
      return value => tenth(ninth(eighth(seventh(sixth(fifth(fourth(third(second(first(value))))))))));
    }
  }
}
