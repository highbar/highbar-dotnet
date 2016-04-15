using Xunit;

namespace Highbar.Algebra.Tests
{
  public class SetoidTests
  {
    [Theory]
    [InlineData(1)]
    [InlineData("test")]
    [InlineData(double.MaxValue)]
    public void Reflexive(object value)
    {
      Assert.True(Setoid.Reflexive(value));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData("test", "test")]
    [InlineData("test1", "test2")]
    [InlineData(double.MaxValue, double.MaxValue)]
    [InlineData(double.MaxValue, double.MinValue)]
    public void Symmetrical(object left, object right)
    {
      Assert.True(Setoid.Symmetrical(left, right));
    }

    public class Transitive
    {
      [Theory]
      [InlineData(1, 1, 1)]
      [InlineData(1, 2, 3)]
      [InlineData("test", "test", "test")]
      [InlineData("test1", "test2", "test3")]
      [InlineData(double.MaxValue, double.MaxValue, double.MaxValue)]
      [InlineData(double.MaxValue, 0.0, double.MinValue)]
      public void True(object left, object middle, object right)
      {
        Assert.True(Setoid.Transitive(left, middle, right));
      }

      [Theory]
      [InlineData(1, 2, 1)]
      [InlineData("test1", "test2", "test1")]
      [InlineData(double.MaxValue, 0.0, double.MaxValue)]
      public void False(object left, object middle, object right)
      {
        Assert.False(Setoid.Transitive(left, middle, right));
      }
    }
  }
}
