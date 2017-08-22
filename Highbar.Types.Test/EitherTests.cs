using System;
using System.Collections.Generic;

using Xunit;
using Moq;

using static Highbar.Functions.Functions;

namespace Highbar.Types.Tests
{
  public class EitherTests
  {
    private static readonly string _testError = "testError";
    private static readonly bool _testValue = true;

    public class EitherStatics
    {
      public class Attempt
      {
        [Fact]
        public void ShouldReturnRightOfTheValue()
        {
          Func<bool> testSupplier = () => _testValue;
          Either<Exception, bool> expectedResult = Either<Exception, bool>.Right<Exception, bool>(_testValue);
          Either<Exception, bool> actualResult = Either<Exception, bool>.Attempt(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnLeftOfTheException()
        {
          Exception testException = new Exception();
          Func<bool> testSupplier = () => { throw testException; };
          Either<Exception, bool> expectedResult = Either<Exception, bool>.Left<Exception, bool>(testException);
          Either<Exception, bool> actualResult = Either<Exception, bool>.Attempt(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class From
      {
        [Fact]
        public void ShouldReturnLeftForNull()
        {
          object testValue = null;
          Either<Exception, object> testEither = Either<Exception, object>.From(testValue);

          Assert.Equal(testEither.IsLeft, true);
        }

        [Fact]
        public void ShouldReturnRightForNonNull()
        {
          Either<Exception, bool> expectedResult = Either<Exception, bool>.Right<Exception, bool>(_testValue);
          Either<Exception, bool> actualResult = Either<Exception, bool>.From(_testValue);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnLeftForNullEither()
        {
          Either<Exception, bool> testEither = null;
          Either<Exception, bool> expectedResult = Either<Exception, bool>.Left<Exception, bool>(null);
          Either<Exception, bool> actualResult = Either<Exception, bool>.From(testEither);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnEitherForEither()
        {
          Either<Exception, bool> testEither = Either<Exception, bool>.Right<Exception, bool>(_testValue);
          Either<Exception, bool> expectedResult = testEither;
          Either<Exception, bool> actualResult = Either<Exception, bool>.From(testEither);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnLeftForNullList()
        {
          List<bool> testList = null;
          Either<Exception, bool> actualResult = Either<Exception, bool>.From(testList);

          Assert.Equal(actualResult.IsLeft, true);
        }

        [Fact]
        public void ShouldReturnLeftForEmptyList()
        {
          List<bool> testList = new List<bool>();
          Either<Exception, bool> actualResult = Either<Exception, bool>.From(testList);

          Assert.Equal(actualResult.IsLeft, true);
        }

        [Fact]
        public void ShouldReturnRightOfFirstValueForPopulatedList()
        {
          List<bool> testList = new List<bool>() { _testValue, !_testValue };
          Either<Exception, bool> expectedResult = Either<Exception, bool>.Right<Exception, bool>(testList[0]);
          Either<Exception, bool> actualResult = Either<Exception, bool>.From(testList);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnLeftForNullMaybe()
        {
          Maybe<object> testMaybe = null;
          Either<Exception, object> actualResult = Either<Exception, object>.From(testMaybe);

          Assert.Equal(actualResult.IsLeft, true);
        }

        [Fact]
        public void ShouldReturnLeftForEmptyMaybe()
        {
          Maybe<object> testMaybe = Maybe<object>.Nothing<object>();
          Either<Exception, object> actualResult = Either<Exception, object>.From(testMaybe);

          Assert.Equal(actualResult.IsLeft, true);
        }

        [Fact]
        public void ShouldReturnRightForMaybe()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Just(_testValue);
          Either<Exception, bool> expectedResult = Either<Exception, bool>.Right<Exception, bool>(_testValue);
          Either<Exception, bool> actualResult = Either<Exception, bool>.From(testMaybe);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class FromLeft
      {
        [Fact]
        public void ShouldReturnValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          string expectedResult = _testError;
          string actualResult = Either<string, bool>.FromLeft(testEither);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldThrowExceptionForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);

          Assert.Throws<ArgumentException>(() => Either<string, bool>.FromLeft(testEither));
        }
      }

      public class FromRight
      {
        [Fact]
        public void ShouldReturnValueForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          bool expectedResult = _testValue;
          bool actualResult = Either<string, bool>.FromRight(testEither);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldThrowExceptionForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);

          Assert.Throws<ArgumentException>(() => Either<string, bool>.FromRight(testEither));
        }
      }

      public class Of
      {
        [Fact]
        public void ShouldReturnRightForRightValue()
        {
          Either<string, bool> actualResult = Either<string, bool>.Of<string, bool>(_testError, _testValue);

          Assert.True(actualResult.IsRight);
        }

        [Fact]
        public void ShouldReturnLeftForRightNull()
        {
          Either<string, bool?> actualResult = Either<string, bool?>.Of<string, bool?>(_testError, null);

          Assert.True(actualResult.IsLeft);
        }
      }

      public class OfNullable
      {
        [Fact]
        public void ShouldReturnLeftForNull()
        {
          Either<Exception, object> actualResult = Either<Exception, object>.OfNullable<object>(null);

          Assert.True(actualResult.IsLeft);
        }

        [Fact]
        public void ShouldReturnRightForValue()
        {
          Either<Exception, bool> actualResult = Either<Exception, bool>.OfNullable<bool>(_testValue);

          Assert.True(actualResult.IsRight);
        }
      }
    }

    public class EitherInstances
    {
      public class Alt
      {
        [Fact]
        public void ShouldReturnLeftForBothLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> testAlt = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> expectedResult = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> actualResult = testEither.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnInstanceForLeftAlt()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, bool> testAlt = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> expectedResult = testEither;
          Either<string, bool> actualResult = testEither.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnInstanceForAltFunction()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Func<Either<string, bool>> testAlt = () => Either<string, bool>.Right<string, bool>(!_testValue);
          Either<string, bool> expectedResult = testEither;
          Either<string, bool> actualResult = testEither.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnAltForRightAlt()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> testAlt = Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, bool> expectedResult = testAlt;
          Either<string, bool> actualResult = testEither.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnAltForRightAltFunction()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Func<Either<string, bool>> testAlt = () => Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, bool> expectedResult = Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, bool> actualResult = testEither.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnInstanceForRightInstance()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, bool> testAlt = Either<string, bool>.Right<string, bool>(!_testValue);
          Either<string, bool> expectedResult = testEither;
          Either<string, bool> actualResult = testEither.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Ap
      {
        [Fact]
        public void ShouldApplyForRightOfValueAndFunction()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, Func<bool, bool>> testApply = Either<string, Func<bool, bool>>.Right<string, Func<bool, bool>>(b => !b);
          Either<string, bool> expectedResult = Either<string, bool>.Right<string, bool>(!_testValue);
          Either<string, bool> actualResult = testEither.Ap(testApply);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotApplyForLeftOfFunction()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, Func<bool, bool>> testApply = Either<string, Func<bool, bool>>.Left<string, Func<bool, bool>>(_testError);
          Either<string, bool> expectedResult = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> actualResult = testEither.Ap(testApply);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotApplyForLeftOfValue()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, Func<bool, bool>> testApply = Either<string, Func<bool, bool>>.Right<string, Func<bool, bool>>(b => !b);
          Either<string, bool> expectedResult = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> actualResult = testEither.Ap(testApply);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnOtherLeftForLeftOfValue()
        {
          string testOther = _testError + "1";
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, Func<bool, bool>> testApply = Either<string, Func<bool, bool>>.Left<string, Func<bool, bool>>(testOther);
          Either<string, bool> expectedResult = Either<string, bool>.Left<string, bool>(testOther);
          Either<string, bool> actualResult = testEither.Ap(testApply);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Bimap
      {
        private readonly Func<string, int> _testLeftMap = Always<string, int>(0);
        private readonly Func<bool, int> _testRightMap = Always<bool, int>(1);

        [Fact]
        public void ShouldMapLeftValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Either<int, int> expectedResult = Either<int, int>.Left<int, int>(0);
          Either<int, int> actualResult = testEither.Bimap(_testLeftMap, _testRightMap);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldMapRightValueForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Either<int, int> expectedResult = Either<int, int>.Right<int, int>(1);
          Either<int, int> actualResult = testEither.Bimap(_testLeftMap, _testRightMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Bind
      {
        [Fact]
        public void ShouldAliasChain()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Func<bool, Either<string, bool>> testChain = v => Either<string, bool>.Right<string, bool>(!v);
          Either<string, bool> expectedResult = testEither.Bind(testChain);
          Either<string, bool> actualResult = testEither.Chain(testChain);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Chain
      {
        [Fact]
        public void ShouldChainForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Func<bool, Either<string, bool>> testChain = v => Either<string, bool>.Right<string, bool>(!v);
          Either<string, bool> expectedResult = Either<string, bool>.Right<string, bool>(!_testValue);
          Either<string, bool> actualResult = testEither.Chain(testChain);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotChainForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Func<bool, Either<string, bool>> testChain = v => Either<string, bool>.Right<string, bool>(!v);
          Either<string, bool> expectedResult = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> actualResult = testEither.Chain(testChain);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Coalesce
      {
        [Fact]
        public void ShouldAliasAlt()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> testAlt = Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, bool> expectedResult = testEither.Alt(testAlt);
          Either<string, bool> actualResult = testEither.Coalesce(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Extend
      {
        [Fact]
        public void ShouldExtendForRight()
        {
           Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
           Func<Either<string, bool>, bool> testExtend = either => either.Map(v => !v).GetOrElse(false);
           Either<string, bool> expectedResult = Either<string, bool>.Right<string, bool>(!_testValue);
           Either<string, bool> actualResult = testEither.Extend(testExtend);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotExtendForLeft()
        {
           Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
           Func<Either<string, bool>, bool> testExtend = either => either.Map(v => !v).GetOrElse(false);
           Either<string, bool> expectedResult = Either<string, bool>.Left<string, bool>(_testError);
           Either<string, bool> actualResult = testEither.Extend(testExtend);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Filter
      {
        [Fact]
        public void ShouldFilterRightToJust()
        {
          string testValue = "test";
          Either<string, string> testEither = Either<string, string>.Right<string, string>(testValue);
          //TODO:  replace with Predicates::AlwaysTrue when I write it.
          Predicate<string> testPredicate = s => true;
          Maybe<string> expectedResult = Maybe<string>.Just(testValue);
          Maybe<string> actualResult = testEither.Filter(testPredicate);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterRightToNothing()
        {
          string testValue = "test";
          Either<string, string> testEither = Either<string, string>.Right<string, string>(testValue);
          //TODO:  replace with Predicates::AlwaysFalse when I write it.
          Predicate<string> testPredicate = s => false;
          Maybe<string> expectedResult = Maybe<string>.Nothing<string>();
          Maybe<string> actualResult = testEither.Filter(testPredicate);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterLeftToNothing()
        {
          Either<string, string> testEither = Either<string, string>.Left<string, string>(_testError);
          //TODO:  replace with Predicates::AlwaysFalse when I write it.
          Predicate<string> testPredicate = s => false;
          Maybe<string> expectedResult = Maybe<string>.Nothing<string>();
          Maybe<string> actualResult = testEither.Filter(testPredicate);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class FilterWithLeftValue
      {
        [Fact]
        public void ShouldFilterRightToRight()
        {
          string testValue = "test";
          Either<string, string> testEither = Either<string, string>.Right<string, string>(testValue);
          //TODO:  replace with Predicates::AlwaysTrue when I write it.
          Predicate<string> testPredicate = s => true;
          Either<string, string> expectedResult = Either<string, string>.Right<string, string>(testValue);
          Either<string, string> actualResult = testEither.Filter(testPredicate, _testError);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterRightToLeft()
        {
          string testValue = "test";
          Either<string, string> testEither = Either<string, string>.Right<string, string>(testValue);
          //TODO:  replace with Predicates::AlwaysFalse when I write it.
          Predicate<string> testPredicate = s => false;
          Either<string, string> expectedResult = Either<string, string>.Left<string, string>(_testError);
          Either<string, string> actualResult = testEither.Filter(testPredicate, _testError);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterRightToLeftWithSupplier()
        {
          string testValue = "test";
          Either<string, string> testEither = Either<string, string>.Right<string, string>(testValue);
          //TODO:  replace with Predicates::AlwaysFalse when I write it.
          Predicate<string> testPredicate = s => false;
          Either<string, string> expectedResult = Either<string, string>.Left<string, string>(_testError);
          Either<string, string> actualResult = testEither.Filter(testPredicate, () => _testError);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterLeftToLeft()
        {
          Either<string, string> testEither = Either<string, string>.Left<string, string>(_testError);
          //TODO:  replace with Predicates::AlwaysFalse when I write it.
          Predicate<string> testPredicate = s => false;
          Either<string, string> expectedResult = testEither;
          Either<string, string> actualResult = testEither.Filter(testPredicate, _testError);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterLeftToLeftWithSupplier()
        {
          Either<string, string> testEither = Either<string, string>.Left<string, string>(_testError);
          //TODO:  replace with Predicates::AlwaysFalse when I write it.
          Predicate<string> testPredicate = s => false;
          Either<string, string> expectedResult = testEither;
          Either<string, string> actualResult = testEither.Filter(testPredicate, () => _testError);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class First
      {
        private readonly Func<string, int> _testLeftMap = Always<string, int>(0);

        [Fact]
        public void ShouldMapLeftValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Either<int, bool> expectedResult = Either<int, bool>.Left<int, bool>(0);
          Either<int, bool> actualResult = testEither.First(_testLeftMap);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotMapLeftValueForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Either<int, bool> expectedResult = Either<int, bool>.Right<int, bool>(_testValue);
          Either<int, bool> actualResult = testEither.First(_testLeftMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class FlatMap
      {
        [Fact]
        public void ShouldAliasChain()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Func<bool, Either<string, bool>> testChain = value => Either<string, bool>.Right<string, bool>(!value);
          Either<string, bool> expectedResult = testEither.FlatMap(testChain);
          Either<string, bool> actualResult = testEither.Chain(testChain);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Fold
      {
        private readonly Func<string, int> _testLeftMap = Always<string, int>(0);
        private readonly Func<bool, int> _testRightMap = Always<bool, int>(1);

        [Fact]
        public void ShouldReturnLeftFoldForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          int expectedResult = 0;
          int actualResult = testEither.Fold(_testLeftMap, _testRightMap);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnRightFoldForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          int expectedResult = 1;
          int actualResult = testEither.Fold(_testLeftMap, _testRightMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class GetLeft
      {
        [Fact]
        public void ShouldReturnLeftValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          string expectedResult = _testError;
          string actualResult = testEither.GetLeft();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnDefaultValueForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          string actualResult = testEither.GetLeft();

          Assert.Equal(default(string), actualResult);
        }
      }

      public class GetRight
      {
        [Fact]
        public void ShouldReturnRightValueForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          bool expectedResult = _testValue;
          bool actualResult = testEither.GetRight();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnDefaultValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          bool actualResult = testEither.GetRight();

          Assert.Equal(default(bool), actualResult);
        }
      }

      public class GetOrElse
      {
        private readonly bool _testOtherValue = !_testValue;

        [Fact]
        public void ShouldReturnValueForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          bool expectedResult = _testValue;
          bool actualResult = testEither.GetOrElse(_testOtherValue);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnOtherValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          bool expectedResult = _testOtherValue;
          bool actualResult = testEither.GetOrElse(_testOtherValue);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class GetOrElseGet
      {
        [Fact]
        public void ShouldReturnValueForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Func<bool> testSupplier = () => !_testValue;
          bool expectedResult = _testValue;
          bool actualResult = testEither.GetOrElseGet(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnMappedOtherValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Func<string, bool> testFunction = error => !_testValue;
          bool expectedResult = !_testValue;
          bool actualResult = testEither.GetOrElseGet(testFunction);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnSuppliedOtherValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Func<bool> testSupplier = () => !_testValue;
          bool expectedResult = !_testValue;
          bool actualResult = testEither.GetOrElseGet(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class GetOrElseThrow
      {
        [Fact]
        public void ShouldReturnValueForRightWithFunction()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Func<string, Exception> testFunction = ignored => new Exception();
          bool expectedResult = _testValue;
          bool actualResult = testEither.GetOrElseThrow(testFunction);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnValueForRightWithSupplier()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Func<Exception> testSupplier = () => new Exception();
          bool expectedResult = _testValue;
          bool actualResult = testEither.GetOrElseThrow(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldThrowForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Func<Exception> testSupplier = () => new Exception();

          Exception exception = Record.Exception(() => testEither.GetOrElseThrow(testSupplier));

          Assert.NotNull(exception);
          Assert.IsType<Exception>(exception);
        }

        [Fact]
        public void ShouldThrowLeftValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Func<string, Exception> testFunction = ignored => new Exception();

          Exception exception = Record.Exception(() => testEither.GetOrElseThrow(testFunction));

          Assert.NotNull(exception);
          Assert.IsType<Exception>(exception);
        }
      }

      public class HashCode
      {
        [Fact]
        public void ShouldReturnLeftHashCodeForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          int expectedResult = _testError.GetHashCode();
          int actualResult = testEither.GetHashCode();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnRightHashCodeForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          int expectedResult = _testValue.GetHashCode();
          int actualResult = testEither.GetHashCode();

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class IfLeft
      {
        [Fact]
        public void ShouldCallIfLeftForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Mock<Action<string>> testConsumer = new Mock<Action<string>>();
          int expectedInvocations = 1;

          testEither.IfLeft(testConsumer.Object);

          testConsumer.Verify(f => f(_testError), Times.Exactly(expectedInvocations));
        }

        [Fact]
        public void ShouldNotCallIfLeftForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Mock<Action<string>> testConsumer = new Mock<Action<string>>();
          int expectedInvocations = 0;

          testEither.IfLeft(testConsumer.Object);

          testConsumer.Verify(f => f(_testError), Times.Exactly(expectedInvocations));
        }
      }

      public class IfRight
      {
        [Fact]
        public void ShouldCallIfRightForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Mock<Action<bool>> testConsumer = new Mock<Action<bool>>();
          int expectedInvocations = 1;

          testEither.IfRight(testConsumer.Object);

          testConsumer.Verify(f => f(_testValue), Times.Exactly(expectedInvocations));
        }

        [Fact]
        public void ShouldNotCallIfRightForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Mock<Action<bool>> testConsumer = new Mock<Action<bool>>();
          int expectedInvocations = 0;

          testEither.IfRight(testConsumer.Object);

          testConsumer.Verify(f => f(_testValue), Times.Exactly(expectedInvocations));
        }
      }

      public class IsRight
      {
        [Fact]
        public void ShouldReturnFalseForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          bool actualResult = testEither.IsRight;

          Assert.False(actualResult);
        }

        [Fact]
        public void ShouldReturnTrueForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          bool actualResult = testEither.IsRight;

          Assert.True(actualResult);
        }
      }

      public class IsLeft
      {
        [Fact]
        public void ShouldReturnFalseForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          bool actualResult = testEither.IsLeft;

          Assert.False(actualResult);
        }

        [Fact]
        public void ShouldReturnTrueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          bool actualResult = testEither.IsLeft;

          Assert.True(actualResult);
        }
      }

      public class LeftMap
      {
        [Fact]
        public void ShouldAliasFirst()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Func<string, int> testLeftMap = Always<string, int>(0);
          Either<int, bool> expectedResult = testEither.First(testLeftMap);
          Either<int, bool> actualResult = testEither.LeftMap(testLeftMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Map
      {
        [Fact]
        public void ShouldMapForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, bool> expectedResult = Either<string, bool>.Right<string, bool>(!_testValue);
          //TODO(lee.crabtree):  replace with Predicates::Negate when I write it.
          Either<string, bool> actualResult = testEither.Map(b => !b);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotMapForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> expectedResult = Either<string, bool>.Left<string, bool>(_testError);
          //TODO(lee.crabtree):  replace with Predicates::Negate when I write it.
          Either<string, bool> actualResult = testEither.Map(b => !b);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Recover
      {
        private bool _testRecover = !_testValue;

        [Fact]
        public void ShouldReturnInstanceForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, bool> expectedResult = testEither;
          Either<string, bool> actualResult = testEither.Recover(_testRecover);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnInstanceForRightFunction()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Func<object, bool> testRecover = ignored => _testRecover;
          Either<string, bool> expectedResult = testEither;
          Either<string, bool> actualResult = testEither.Recover(testRecover);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnInstanceForRightSupplier()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Func<bool> testRecover = () => _testRecover;
          Either<string, bool> expectedResult = testEither;
          Either<string, bool> actualResult = testEither.Recover(testRecover);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> expectedResult = Either<string, bool>.Right<string, bool>(_testRecover);
          Either<string, bool> actualResult = testEither.Recover(_testRecover);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnValueFromFunctionForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Func<object, bool> testRecover = ignored =>  !_testValue;
          Either<string, bool> expectedResult = Either<string, bool>.Right<string, bool>(!_testValue);
          Either<string, bool> actualResult = testEither.Recover(testRecover);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnValueFromSupplierForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Func<bool> testRecover = () =>  !_testValue;
          Either<string, bool> expectedResult = Either<string, bool>.Right<string, bool>(!_testValue);
          Either<string, bool> actualResult = testEither.Recover(testRecover);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Reduce
      {
        [Fact]
        public void ShouldAliasFold()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Func<string, int> testLeftMap = Always<string, int>(0);
          Func<bool, int> testRightMap = Always<bool, int>(1);
          int expectedResult = testEither.Fold(testLeftMap, testRightMap);
          int actualResult = testEither.Reduce(testLeftMap, testRightMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class RightMap
      {
        [Fact]
        public void ShouldAliasSecond()
        {
          Either<string, bool> testEither = Either<string, bool>.Right< string, bool>(_testValue);
          Func<bool, int> testRightMap = Always<bool, int>(1);
          Either<string, int> expectedResult = testEither.Second(testRightMap);
          Either<string, int> actualResult = testEither.RightMap(testRightMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Second
      {
        private readonly Func<bool, int> _testRightMap = Always<bool, int>(1);

        [Fact]
        public void ShouldMapRightValueForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, int> expectedResult = Either<string, int>.Right<string, int>(1);
          Either<string, int> actualResult = testEither.Second(_testRightMap);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotMapRightValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, int> expectedResult = Either<string, int>.Left<string, int>(_testError);
          Either<string, int> actualResult = testEither.Second(_testRightMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Tap
      {
        [Fact]
        public void ShouldCallIfRightForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Mock<Action<string>> testLeftConsumer = new Mock<Action<string>>();
          Mock<Action<bool>> testRightConsumer = new Mock<Action<bool>>();
          int expectedLeftInvocations = 0;
          int expectedRightInvocations = 1;

          testEither.Tap(testLeftConsumer.Object, testRightConsumer.Object);

          testLeftConsumer.Verify(f => f(_testError), Times.Exactly(expectedLeftInvocations));
          testRightConsumer.Verify(f => f(_testValue), Times.Exactly(expectedRightInvocations));
        }

        [Fact]
        public void ShoulCallIfLeftForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Mock<Action<string>> testLeftConsumer = new Mock<Action<string>>();
          Mock<Action<bool>> testRightConsumer = new Mock<Action<bool>>();
          int expectedLeftInvocations = 1;
          int expectedRightInvocations = 0;

          testEither.Tap(testLeftConsumer.Object, testRightConsumer.Object);

          testLeftConsumer.Verify(f => f(_testError), Times.Exactly(expectedLeftInvocations));
          testRightConsumer.Verify(f => f(_testValue), Times.Exactly(expectedRightInvocations));
        }
      }

      public class ToList
      {
        [Fact]
        public void ShouldReturnListForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          List<bool> expectedList = new List<bool> { _testValue };
          IList<bool> actualList = testEither.ToList();

          Assert.Equal(expectedList, actualList);
        }

        [Fact]
        public void ShouldReturnEmptyListForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          List<bool> expectedList = new List<bool>();
          IList<bool> actualList = testEither.ToList();

          Assert.Equal(expectedList, actualList);
        }
      }

      public class ToMaybe
      {
        [Fact]
        public void ShouldReturnNothingForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
          Maybe<bool> actualResult = testEither.ToMaybe();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnJustForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          Maybe<bool> expectedResult = Maybe<bool>.Just(_testValue);
          Maybe<bool> actualResult = testEither.ToMaybe();

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class String
      {
        [Fact]
        public void ShouldReturnRightValueForRight()
        {
          Either<string, bool> testEither = Either<string, bool>.Right<string, bool>(_testValue);
          string expectedResult = "Right{" + _testValue + "}";
          string actualResult = testEither.ToString();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnLeftValueForLeft()
        {
          Either<string, bool> testEither = Either<string, bool>.Left<string, bool>(_testError);
          string expectedResult = "Left{" + _testError + "}";
          string actualResult = testEither.ToString();

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class ToValidation
      {
        
      }
    }
  }
}
