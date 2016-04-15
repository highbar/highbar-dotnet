using System.Collections.Generic;

using Xunit;

namespace Highbar.Algebra.Tests
{
  public class MonoidTests
  {
    private static readonly string[] _data = new []{ "one", "two", "three" };
    private static List<string> AsList() => new List<string>(_data);
    private static SortedSet<string> AsSet() => new SortedSet<string>(_data);
    private static string[] AsArray() => AsList().ToArray();

    public class RightIdentity
    {
      [Fact]
      public void List()
      {
        Assert.True(Monoid.RightIdentity(AsList()));
      }

      [Fact]
      public void Set()
      {
        Assert.True(Monoid.RightIdentity(AsSet()));
      }

      [Fact]
      public void Array()
      {
        Assert.True(Monoid.RightIdentity(AsArray()));
      }
    }

    public class LeftIdentity
    {
      [Fact]
      public void List()
      {
        Assert.True(Monoid.LeftIdentity(AsList()));
      }

      [Fact]
      public void Set()
      {
        Assert.True(Monoid.LeftIdentity(AsSet()));
      }

      [Fact]
      public void Array()
      {
        Assert.True(Monoid.LeftIdentity(AsArray()));
      }
    }
  }
}
