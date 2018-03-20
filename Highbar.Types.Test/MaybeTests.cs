using System;
using System.Collections.Generic;

using Xunit;
using Moq;

using Highbar.Types;

namespace Highbar.Types.Tests
{
  public class MaybeTests
  {
    private static readonly bool _testValue = false;
    private static readonly Maybe<bool> _testMaybe = Maybe<bool>.Just(_testValue);

    public class MaybeStatics
    {
      public class Attempt
      {
        [Fact]
        public void ShouldReturnRightOfTheValue()
        {
          Func<bool> testSupplier = () => _testValue;
          Maybe<bool> expectedResult = Maybe<bool>.Just(_testValue);
          Maybe<bool> actualResult = Maybe<bool>.Attempt(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnLeftOfTheThrowable()
        {
          Func<bool> testSupplier = () => { throw new Exception(); };
          Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
          Maybe<bool> actualResult = Maybe<bool>.Attempt(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Empty
      {
        [Fact]
        public void ShouldReturnNothing()
        {
          Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
          Maybe<bool> actualResult = Maybe<bool>.Empty<bool>();

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class From
      {
        [Fact]
        public void ShouldReturnNothingForNull()
        {
          object testValue = null;
          Maybe<object> expectedResult = Maybe<object>.Nothing<object>();
          Maybe<object> actualResult = Maybe<object>.From(testValue);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnForNonNull()
        {
          Maybe<bool> expectedResult = _testMaybe;
          Maybe<bool> actualResult = Maybe<bool>.From(_testValue);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnNothingForEmptyList()
        {
          List<object> testList = new List<object>();
          Maybe<object> expectedResult = Maybe<object>.Nothing<object>();
          Maybe<object> actualResult = Maybe<object>.From<object>(testList);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnJustOfFirstValueForPopulatedList()
        {
          List<bool> testList = new List<bool> { _testValue, !_testValue };
          Maybe<bool> expectedResult = Maybe<bool>.Just(testList[0]);
          Maybe<bool> actualResult = Maybe<bool>.From<bool>(testList);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnMaybeForMaybe()
        {
          Maybe<bool> expectedResult = _testMaybe;
          Maybe<bool> actualResult = Maybe<bool>.From<bool>(_testMaybe);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class FromJust
      {
        [Fact]
        public void ShouldReturnValueForJust()
        {
          bool expectedResult = _testValue;
          bool actualResult = Maybe<bool>.FromJust(_testMaybe);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldThrowForNothing()
        {
          Exception exception = Record.Exception(() => Maybe<bool>.FromJust(Maybe<bool>.Nothing<bool>()));
          Assert.NotNull(exception);
          Assert.IsType<ArgumentException>(exception);
        }
      }

      public class MaybeMap
      {
        private readonly string _defaultValue = "default";
        private readonly Func<bool, string> _defaultMapper = b => b.ToString();

        [Fact]
        public void ShouldApplyMapperForJust()
        {
          string actualResult = Maybe<string>.MaybeMap(_defaultValue, _defaultMapper, _testMaybe);
          string expectedResult = _testValue.ToString();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnDefaultValueForNull()
        {
          Maybe<bool> testMaybe = null;
          string expectedResult = _defaultValue;
          string actualResult = Maybe<string>.MaybeMap(_defaultValue, _defaultMapper, testMaybe);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnDefaultValueForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          string expectedResult = _defaultValue;
          string actualResult = Maybe<string>.MaybeMap(_defaultValue, _defaultMapper, testMaybe);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class MaybeMapCurried
      {
        private string _defaultValue = "default";
        private readonly Func<bool, string> _defaultMapper = b => b.ToString();

        [Fact]
        public void ShouldApplyMapperForJust()
        {
          string expectedResult = _testValue.ToString();
          string actualResult = Maybe<string>.MaybeMap(_defaultValue, _defaultMapper)(_testMaybe);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnDefaultValueForNull()
        {
          Maybe<bool> testMaybe = null;
          string expectedResult = _defaultValue;
          string actualResult = Maybe<string>.MaybeMap(_defaultValue, _defaultMapper)(testMaybe);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnDefaultValueForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          string expectedResult = _defaultValue;
          string actualResult = Maybe<string>.MaybeMap(_defaultValue, _defaultMapper)(testMaybe);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Of
      {
        [Fact]
        public void ShouldReturnJustForValue()
        {
          Maybe<bool> expectedResult = _testMaybe;
          Maybe<bool> actualResult = Maybe<bool>.Of(_testValue);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class OfNullable
      {
        [Fact]
        public void ShouldReturnNothingForNull()
        {
          Maybe<object> expectedResult = Maybe<object>.Nothing<object>();
          Maybe<object> actualResult = Maybe<object>.OfNullable<object>(null);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnJustForValue()
        {
          Maybe<bool> expectedResult = _testMaybe;
          Maybe<bool> actualResult = Maybe<bool>.OfNullable(_testValue);

          Assert.Equal(expectedResult, actualResult);
        }
      }
    }

    public class MaybeInstances
    {
      public class Alt
      {
        [Fact]
        public void ShouldReturnNothingForBothNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          Maybe<bool> testAlt = Maybe<bool>.Nothing<bool>();
          Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
          Maybe<bool> actualResult = testMaybe.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnInstanceForNothingAlt()
        {
          Maybe<bool> testAlt = Maybe<bool>.Nothing<bool>();
          Maybe<bool> expectedResult = _testMaybe;
          Maybe<bool> actualResult = _testMaybe.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnAltForJustAlt()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          Maybe<bool> testAlt = Maybe<bool>.Just(_testValue);
          Maybe<bool> expectedResult = testAlt;
          Maybe<bool> actualResult = testMaybe.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnInstanceForJustInstance()
        {
          Maybe<bool> testAlt = Maybe<bool>.Just(!_testValue);
          Maybe<bool> expectedResult = _testMaybe;
          Maybe<bool> actualResult = _testMaybe.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }
      }
      public class Ap
      {
        [Fact]
        public void ShouldApplyForJustOfValueAndFunction()
          {
            Maybe<Func<bool, bool>> testApply = Maybe<Func<bool, bool>>.Just<Func<bool, bool>>(b => !b);
            Maybe<bool> expectedResult = Maybe<bool>.Just(!_testValue);
            Maybe<bool> actualResult = _testMaybe.Ap(testApply);

            Assert.Equal(expectedResult, actualResult);
          }

        [Fact]
        public void ShouldNotApplyForNothingOfFunction()
          {
            Maybe<Func<bool, bool>> testApply = Maybe<Func<bool, bool>>.Nothing<Func<bool, bool>>();
            Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
            Maybe<bool> actualResult = _testMaybe.Ap(testApply);

            Assert.Equal(expectedResult, actualResult);
          }

        [Fact]
        public void ShouldNotApplyForNothingOfValue()
          {
            Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
            Maybe<Func<bool, bool>> testApply = Maybe<bool>.Just<Func<bool, bool>>(b => !b);
            Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
            Maybe<bool> actualResult = testMaybe.Ap(testApply);

            Assert.Equal(expectedResult, actualResult);
          }
      }

      public class Bind
      {
        [Fact]
        public void ShouldAliasChain()
          {
            Func<bool, Maybe<bool>> testChain = value => Maybe<bool>.Just(!value);
            Maybe<bool> expectedResult = _testMaybe.Chain(testChain);
            Maybe<bool> actualResult = _testMaybe.Bind(testChain);

            Assert.Equal(expectedResult, actualResult);
          }
      }

      public class Chain
      {
        [Fact]
        public void ShouldChainForJust()
          {
            Func<bool, Maybe<bool>> testChain = value => Maybe<bool>.Just(!value);
            Maybe<bool> expectedResult = Maybe<bool>.Just(!_testValue);
            Maybe<bool> actualResult = _testMaybe.Chain(testChain);

            Assert.Equal(expectedResult, actualResult);
          }

        [Fact]
        public void ShouldNotChainForNothing()
          {
            Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
            Func<bool, Maybe<bool>> testChain = value => Maybe<bool>.Just(!value);
            Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
            Maybe<bool> actualResult = testMaybe.Chain(testChain);

            Assert.Equal(expectedResult, actualResult);
          }
      }

      public class CheckedMap
      {
        private readonly Func<bool, bool> _testMap = b => !b;
        private readonly Func<bool, bool> _testThrowMap = value => throw new Exception();

        [Fact]
        public void ShouldMapForJust()
          {
            Maybe<bool> testMaybe = Maybe<bool>.Just(_testValue);
            Maybe<bool> expectedResult = Maybe<bool>.Just(!_testValue);
            Maybe<bool> actualResult = testMaybe.CheckedMap(_testMap);

            Assert.Equal(expectedResult, actualResult);
          }

        [Fact]
        public void ShouldNotMapForNothing()
          {
            Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
            Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
            Maybe<bool> actualResult = testMaybe.CheckedMap(_testMap);

            Assert.Equal(expectedResult, actualResult);
          }

        [Fact]
        public void ShouldMapThrownToNothing()
          {
            Maybe<bool> testMaybe = Maybe<bool>.Just(_testValue);
            Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
            Maybe<bool> actualResult = testMaybe.CheckedMap(_testThrowMap);

            Assert.Equal(expectedResult, actualResult);
          }
      }

      public class Coalesce
      {
        [Fact]
        public void ShouldAliasAlt()
          {
            Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
            Maybe<bool> expectedResult = testMaybe.Alt(_testMaybe);
            Maybe<bool> actualResult = testMaybe.Coalesce(_testMaybe);

            Assert.Equal(expectedResult, actualResult);
          }
      }

      public class Extend
      {
        [Fact]
        public void ShouldExtendForJust()
          {
            bool testDefaultValue = false;
            Func<Maybe<bool>, bool> testExtend = maybe => maybe
              //TODO(lee.crabtree): change to Predicates::Negate when you write it.
              .Map(b => !b)
              .GetOrElse(testDefaultValue);
            Maybe<bool> expectedResult = Maybe<bool>.Just(!_testValue);
            Maybe<bool> actualResult = _testMaybe.Extend(testExtend);

            Assert.Equal(expectedResult, actualResult);
          }

        [Fact]
        public void ShouldNotExtendForNothing()
          {
            bool testDefaultValue = false;
            Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
            Func<Maybe<bool>, bool> testExtend = maybe => maybe
              //TODO(lee.crabtree): change to Predicates::Negate when you write it.
              .Map(b => !b)
              .GetOrElse(testDefaultValue);
            Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
            Maybe<bool> actualResult = testMaybe.Extend(testExtend);

            Assert.Equal(expectedResult, actualResult);
          }
      }

      public class Filter
      {
        [Fact]
        public void ShouldFilterJustToJust()
          {
            string testValue = "test";
            Maybe<string> testMaybe = Maybe<string>.Just(testValue);
            //TODO(lee.crabtree): change to Predicates::AlwaysTrue when you write it.
            Predicate<string> testPredicate = s => true;
            Maybe<string> expectedResult = testMaybe;
            Maybe<string> actualResult = testMaybe.Filter(testPredicate);

            Assert.Equal(expectedResult, actualResult);
          }

        [Fact]
        public void ShouldFilterJustToNothing()
          {
            string testValue = "test";
            Maybe<string> testMaybe = Maybe<string>.Just(testValue);
            //TODO(lee.crabtree): change to Predicates::AlwaysFalse when you write it.
            Predicate<string> testPredicate = s => false;
            Maybe<string> expectedResult = Maybe<string>.Nothing<string>();
            Maybe<string> actualResult = testMaybe.Filter(testPredicate);

            Assert.Equal(expectedResult, actualResult);
          }

        [Fact]
        public void ShouldNotFilterNothingToJust()
          {
            Maybe<string> testMaybe = Maybe<string>.Nothing<string>();
            //TODO(lee.crabtree): change to Predicates::AlwaysFalse when you write it.
            Predicate<string> testPredicate = s => false;
            Maybe<string> expectedResult = testMaybe;
            Maybe<string> actualResult = testMaybe.Filter(testPredicate);

            Assert.Equal(expectedResult, actualResult);
          }
      }

      public class FlatMap
      {
        [Fact]
        public void ShouldAliasChain()
          {
            Func<bool, Maybe<bool>> testChain = value => Maybe<bool>.Just(!value);
            Maybe<bool> expectedResult = _testMaybe.Chain(testChain);
            Maybe<bool> actualResult = _testMaybe.FlatMap(testChain);

            Assert.Equal(expectedResult, actualResult);
          }
      }

      public class FoldLeft
      {
        private static readonly string _testInitialValue = "initialValue";
        private static readonly string _expectedResult = "expectedResult";
        private static readonly Func<string, bool, string> _testLeftFold = (initialValue, underlyingValue) => _expectedResult;

        [Fact]
        public void ShouldReturnValueForJust()
          {
            string actualResult = _testMaybe.FoldLeft(_testLeftFold, _testInitialValue);

            Assert.Equal(_expectedResult, actualResult);
          }

        [Fact]
        public void ShouldReturnValueForNothing()
          {
            Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
            string actualResult = testMaybe.FoldLeft(_testLeftFold, _testInitialValue);

            Assert.Equal(_expectedResult, actualResult);
          }
      }

      public class FoldRight
      {
        private static readonly string _testInitialValue = "initialValue";
        private static readonly string _expectedResult = "expectedResult";
        private static readonly Func<bool, string, string> _testRightFold = (underlyingValue, initialValue) => _expectedResult;

        [Fact]
        public void ShouldReturnValueForJust()
          {
            string actualResult = _testMaybe.FoldRight(_testRightFold, _testInitialValue);

            Assert.Equal(_expectedResult, actualResult);
          }

        [Fact]
        public void ShouldReturnValueForNothing()
          {
            Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
            string actualResult = testMaybe.FoldRight(_testRightFold, _testInitialValue);

            Assert.Equal(_expectedResult, actualResult);
          }
      }

      public class GetOrElse
      {
        [Fact]
        public void ShouldReturnValueForJust()
        {
          bool testDefaultValue = !_testValue;
          bool expectedResult = _testValue;
          bool actualResult = _testMaybe.GetOrElse(testDefaultValue);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnOtherValueForNothing()
        {
          bool testDefaultValue = !_testValue;
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          bool expectedResult = !_testValue;
          bool actualResult = testMaybe.GetOrElse(testDefaultValue);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class GetOrElseGet
      {
        [Fact]
        public void ShouldReturnValueForJust()
        {
          //TODO(lee.crabtree): replace with Suppliers.Always(!testValue) when I write it.
          Func<bool> testSupplier = () => !_testValue;
          bool expectedResult = _testValue;
          bool actualResult = _testMaybe.GetOrElseGet(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnOtherValueForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          //TODO(lee.crabtree): replace with Suppliers.Always(!testValue) when I write it.
          Func<bool> testSupplier = () => !_testValue;
          bool expectedResult = !_testValue;
          bool actualResult = testMaybe.GetOrElseGet(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class GetOrElseThrow
      {
        [Fact]
        public void ShouldReturnValueForJust()
        {
          Func<Exception> testSupplier = () => new Exception();
          bool expectedResult = _testValue;
          bool actualResult = _testMaybe.GetOrElseThrow(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldThrowForNothing()
        {
          Func<Exception> testSupplier = () => new Exception();
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          Exception exception = Record.Exception(() => testMaybe.GetOrElseThrow(testSupplier));

          Assert.NotNull(exception);
          Assert.IsType<Exception>(exception);
        }
      }

      public class HashCode
      {
        [Fact]
        public void ShouldReturn1ForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          int expectedResult = 1;
          int actualResult = testMaybe.GetHashCode();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnValueHashCodeForJust()
        {
          int expectedResult = _testValue.GetHashCode();
          int actualResult = _testMaybe.GetHashCode();

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class IfJust
      {
        [Fact]
        public void ShouldCallIfJustForJust()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Just(_testValue);
          Mock<Action<bool>> testConsumer = new Mock<Action<bool>>();
          int expectedInvocations = 1;

          testMaybe.IfJust(testConsumer.Object);

          testConsumer.Verify(f => f(_testValue), Times.Exactly(expectedInvocations));
        }

        [Fact]
        public void ShouldNotCallIfJustForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          Mock<Action<bool>> testConsumer = new Mock<Action<bool>>();
          int expectedInvocations = 0;

          testMaybe.IfJust(testConsumer.Object);

          testConsumer.Verify(f => f(_testValue), Times.Exactly(expectedInvocations));
        }
      }

      public class IfNothing
      {
        [Fact]
        public void ShouldCallIfNothingForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          Mock<Action> testRunnable = new Mock<Action>();
          int expectedInvocations = 1;

          testMaybe.IfNothing(testRunnable.Object);

          testRunnable.Verify(f => f(), Times.Exactly(expectedInvocations));
        }

        [Fact]
        public void ShouldNotCallIfNothingForJust()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Just(_testValue);
          Mock<Action> testRunnable = new Mock<Action>();
          int expectedInvocations = 0;

          testMaybe.IfNothing(testRunnable.Object);

          testRunnable.Verify(f => f(), Times.Exactly(expectedInvocations));
        }
      }

      public class IsJust
      {
        [Fact]
        public void ShouldReturnTrueForJust()
        {
          Assert.True(_testMaybe.IsJust());
        }

        [Fact]
        public void ShouldReturnFalseForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();

          Assert.False(testMaybe.IsJust());
        }
      }

      public class IsNothing
      {
        [Fact]
        public void ShouldReturnFalseForJust()
        {
          Assert.False(_testMaybe.IsNothing());
        }

        [Fact]
        public void ShouldReturnTrueForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();

          Assert.True(testMaybe.IsNothing());
        }
      }

      public class Map
      {
        //TODO(lee.crabtree): replace with Predicates::Negate when I write it.
        private readonly Func<bool, bool> _testMap = b => !b;

        [Fact]
        public void ShouldMapForJust()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Just(_testValue);
          Maybe<bool> expectedResult = Maybe<bool>.Just(!_testValue);
          Maybe<bool> actualResult = testMaybe.Map(_testMap);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotMapForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
          Maybe<bool> actualResult = testMaybe.Map(_testMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Recover
      {
        private readonly bool _testRecover = !_testValue;

        [Fact]
        public void ShouldReturnValueForJust()
        {
          Maybe<bool> expectedResult = _testMaybe;
          Maybe<bool> actualResult = _testMaybe.Recover(_testRecover);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnValueForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          Maybe<bool> expectedResult = Maybe<bool>.Just(_testRecover);
          Maybe<bool> actualResult = testMaybe.Recover(_testRecover);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Tap
      {
        [Fact]
        public void ShouldCallIfJustForJust()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Just<bool>(true);
          Mock<Action> testIfNothingRunnable = new Mock<Action>();
          Mock<Action<bool>> testIfJustAction = new Mock<Action<bool>>();
          int expectedIfNothingInvocations = 0;
          int expectedIfJustInvocations = 1;

          testMaybe.Tap(testIfNothingRunnable.Object, testIfJustAction.Object);

          testIfNothingRunnable.Verify(f => f(), Times.Exactly(expectedIfNothingInvocations));
          testIfJustAction.Verify(f => f(It.IsAny<bool>()), Times.Exactly(expectedIfJustInvocations));
        }

        [Fact]
        public void ShouldCallIfNothingForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          Mock<Action> testIfNothingRunnable = new Mock<Action>();
          Mock<Action<bool>> testIfJustAction = new Mock<Action<bool>>();
          int expectedIfNothingInvocations = 1;
          int expectedIfJustInvocations = 0;

          testMaybe.Tap(testIfNothingRunnable.Object, testIfJustAction.Object);

          testIfNothingRunnable.Verify(f => f(), Times.Exactly(expectedIfNothingInvocations));
          testIfJustAction.Verify(f => f(It.IsAny<bool>()), Times.Exactly(expectedIfJustInvocations));
        }
      }

      public class ToEither
      {
        [Fact]
        public void ShouldReturnRightForJust()
        {
          Either<Exception, bool> expectedResult = Either<Exception, bool>.Right<Exception, bool>(_testValue);
          Either<Exception, bool> actualResult = _testMaybe.ToEither();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnLeftForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          Either<Exception, bool> actualResult = testMaybe.ToEither();

          Assert.True(actualResult.IsLeft);
        }
      }

      public class ToList
      {
        [Fact]
        public void ShouldReturnListForJust()
        {
          IList<bool> expectedResult = new List<bool> { _testValue };
          IList<bool> actualResult = _testMaybe.ToList();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnEmptyListForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          IList<bool> expectedResult = new List<bool>();
          IList<bool> actualResult = testMaybe.ToList();

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class ToValidation
      {

      }

      public class String
      {
        [Fact]
        public void ShouldReturnMaybeValueForJust()
        {
          string expectedResult = "Just{" + _testValue + "}";
          string actualResult = _testMaybe.ToString();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnNothingForNothing()
        {
          Maybe<object> testMaybe = Maybe<object>.Nothing<object>();
          string expectedResult = "Nothing";
          string actualResult = testMaybe.ToString();

          Assert.Equal(expectedResult, actualResult);
        }
      }
    }
  }
}
