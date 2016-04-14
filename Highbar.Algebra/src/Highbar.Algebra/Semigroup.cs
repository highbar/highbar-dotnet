using System.Collections.Generic;
using System.Linq;

namespace Highbar.Algebra
{
  public static class Semigroup
  {
    public static TripleResult<IEnumerable<L>,IEnumerable<M>,IEnumerable<R>> Associative<L,M,R>(
      IEnumerable<L> left, IEnumerable<M> middle, IEnumerable<R> right
    )
    {
      return new TripleResult<IEnumerable<L>,IEnumerable<M>,IEnumerable<R>>(Laws.Associative(
        (IEnumerable<object>) left,
        (IEnumerable<object>) middle,
        (IEnumerable<object>) right
      ), left, middle, right);
    }

    private static class Laws
    {
      internal static bool Associative(
        IEnumerable<object> left, IEnumerable<object> middle, IEnumerable<object> right
      ) => left.Concat(middle).Concat(right).SequenceEqual(left.Concat(middle.Concat(right)));
    }
  }
}
