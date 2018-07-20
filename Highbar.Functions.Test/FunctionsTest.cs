using System;

using Xunit;

using static Highbar.Functions.Functions;

namespace Highbar.Functions.Test
{
  public class FunctionsTests
  {
    private static readonly string _testString = "string";
    private static readonly bool _testInputA = true;
    private static readonly Func<bool, int> _testAtoB = value => value ? 1 : 0;
    private static readonly Func<int, string> _testBtoC = value => value == 1 ? "one" : "zero";
    private static readonly Func<string, int> _testCtoD = value => value == "one" ? 1 : 0;
    private static readonly Func<int, bool> _testDtoE = value => value == 1 ? true : false;
    private static readonly Func<bool, string> _testEtoF = value => value == true ? "true" : "false";
    private static readonly Func<string, bool> _testFtoG = value => value == "true" ? true : false;

    public class Always
    {
      [Fact]
      public void shouldReturnTheAlwaysValue()
      {
        bool expectedValue = _testInputA;
        bool actualValue = Always<int, bool>(_testInputA)(1);

        Assert.Equal(expectedValue, actualValue);
      }

      [Fact]
      public void ShouldReturnTheAlwaysValue()
      {
        string expectedValue = _testString;
        string actualValue = Always(_testString)(new object());

        Assert.Equal(expectedValue, actualValue);
      }
    }

    public class AlwaysSupply
    {
      [Fact]
      public void ShouldReturnTheAlwaysValue()
      {
        string expectedValue = _testString;
        string actualValue = AlwaysSupply(_testString)();

        Assert.Equal(expectedValue, actualValue);
      }
    }

    public class Pipe
    {
      [Fact]
      public void shouldPipe2()
      {
        string expectedResult = "one";
        string actualResult = Pipe(_testAtoB, _testBtoC)(_testInputA);

        Assert.Equal(expectedResult, actualResult);
      }

      [Fact]
      public void shouldPipe3()
      {
        int expectedResult = 1;
        int actualResult = Pipe(_testAtoB, _testBtoC, _testCtoD)(_testInputA);

        Assert.Equal(expectedResult, actualResult);
      }

      [Fact]
      public void shouldPipe4()
      {
        bool expectedResult = true;
        bool actualResult = Pipe(_testAtoB,
                                _testBtoC,
                                _testCtoD,
                                _testDtoE)(_testInputA);

        Assert.Equal(expectedResult, actualResult);
      }

      [Fact]
      public void shouldPipe5()
      {
        string expectedResult = "true";
        string actualResult = Pipe(_testAtoB,
                                   _testBtoC,
                                   _testCtoD,
                                   _testDtoE,
                                   _testEtoF)(_testInputA);

        Assert.Equal(expectedResult, actualResult);
      }

      [Fact]
      public void shouldPipe6()
      {
        bool expectedResult = true;
        bool actualResult = Pipe(_testAtoB,
                                 _testBtoC,
                                 _testCtoD,
                                 _testDtoE,
                                 _testEtoF,
                                 _testFtoG)(_testInputA);

        Assert.Equal(expectedResult, actualResult);
      }

      [Fact]
      public void shouldPipe7()
      {
        int expectedResult = 1;
        int actualResult = Pipe(_testAtoB,
                                 _testBtoC,
                                 _testCtoD,
                                 _testDtoE,
                                 _testEtoF,
                                 _testFtoG,
                                 _testAtoB)(_testInputA);

        Assert.Equal(expectedResult, actualResult);
      }

      [Fact]
      public void shouldPipe8()
      {
        string expectedResult = "one";
        string actualResult = Pipe(_testAtoB,
                                   _testBtoC,
                                   _testCtoD,
                                   _testDtoE,
                                   _testEtoF,
                                   _testFtoG,
                                   _testAtoB,
                                   _testBtoC)(_testInputA);

        Assert.Equal(expectedResult, actualResult);
      }

      [Fact]
      public void shouldPipe9()
      {
        int expectedResult = 1;
        int actualResult = Pipe(_testAtoB,
                                _testBtoC,
                                _testCtoD,
                                _testDtoE,
                                _testEtoF,
                                _testFtoG,
                                _testAtoB,
                                _testBtoC,
                                _testCtoD)(_testInputA);

        Assert.Equal(expectedResult, actualResult);
      }

      [Fact]
      public void shouldPipe10()
      {
        bool expectedResult = true;
        bool actualResult = Pipe(_testAtoB,
                                 _testBtoC,
                                 _testCtoD,
                                 _testDtoE,
                                 _testEtoF,
                                 _testFtoG,
                                 _testAtoB,
                                 _testBtoC,
                                 _testCtoD,
                                 _testDtoE)(_testInputA);

        Assert.Equal(expectedResult, actualResult);
      }
    }
  }
}
