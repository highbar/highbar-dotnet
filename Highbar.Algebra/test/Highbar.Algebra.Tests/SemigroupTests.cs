using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Highbar.Algebra.Tests
{
  public class SemigroupTests
  {
    private static readonly string[] _data = new []{ "one", "two", "three" };
    private static List<string> AsList() => new List<string>(_data);
    private static SortedSet<string> AsSet() => new SortedSet<string>(_data);
    private static string[] AsArray() => AsList().ToArray();

    public class TrueForCanonicalEnumerables
    {
      [Fact]
      public void List()
      {
        Assert.True(Semigroup.Associative(AsList(), AsList(), AsList()));
      }

      [Fact]
      public void Set()
      {
        Assert.True(Semigroup.Associative(AsSet(), AsSet(), AsSet()));
      }

      [Fact]
      public void Array()
      {
        Assert.True(Semigroup.Associative(AsArray(), AsArray(), AsArray()));
      }
    }

    [Fact]
    public void TrueForMixtures()
    {
      Assert.True(Semigroup.Associative(AsList(), AsSet(), AsArray()));
    }
  }
}
