using System.Collections.Generic;
using System.Linq;

namespace Highbar.Algebra
{
  public static class Monoid
  {
    public static SingleResult<IEnumerable<X>> Empty<X>() =>
      new SingleResult<IEnumerable<X>>(true, Enumerable.Empty<X>());

    public static SingleResult<IEnumerable<X>> RightIdentity<X>(IEnumerable<X> items) =>
      new SingleResult<IEnumerable<X>>(Laws.RightIdentity(items), items);

    public static SingleResult<IEnumerable<X>> LeftIdentity<X>(IEnumerable<X> items) =>
      new SingleResult<IEnumerable<X>>(Laws.LeftIdentity(items), items);

    private static class Laws
    {
      internal static bool RightIdentity<X>(IEnumerable<X> items) =>
        Semigroup.Associative(items, items, items) &&
        items.Concat(Monoid.Empty<X>().Value).SequenceEqual(items);

      internal static bool LeftIdentity<X>(IEnumerable<X> items) =>
        Semigroup.Associative(items, items, items) &&
        Monoid.Empty<X>().Value.Concat(items).SequenceEqual(items);
    }
  }
}
