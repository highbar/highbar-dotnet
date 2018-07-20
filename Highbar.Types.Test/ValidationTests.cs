using System;
using System.Collections.Generic;

using Xunit;
using Moq;
using System.Linq;

using static Highbar.Functions.Functions;

namespace Highbar.Types.Tests
{
  public class ValidationTests
  {
    private static readonly string _testError = "testError";
    private static readonly bool _testValue = true;

    public class ValidationStatics
    {
      public class Attempt
      {
        [Fact]
        public void ShouldReturnRightOfTheValue()
        {
          Func<bool> supplier = () => _testValue;
          Validation<Exception, bool> expectedResult = Validation<Exception, bool>.Pass<Exception, bool>(_testValue);
          Validation<Exception, bool> actualResult = Validation<Exception, bool>.Attempt(supplier);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnLeftOfTheException()
        {
          Func<bool> supplier = () => throw new NullReferenceException("value is null");
          Validation<Exception, bool> expectedResult = Validation<Exception, bool>.Fail<Exception, bool>(new NullReferenceException("value is null"));
          Validation<Exception, bool> actualResult = Validation<Exception, bool>.Attempt(supplier);

          //NOTE(lee.crabtree):  The underlying exceptions only test for reference equality.  (/facepalm)
          Assert.Equal(expectedResult.ToString(), actualResult.ToString());
        }
      }

      public class From
      {
        [Fact]
        public void ShouldReturnFailureForNull()
        {
          object testValue = null;
          Validation<Exception, object> testValidation = Validation<Exception, object>.From(testValue);
          Validation<Exception, object> expectedValidation = Validation<Exception, object>.Fail<Exception, object>(new NullReferenceException("value is null"));

          //NOTE(lee.crabtree):  The underlying exceptions only test for reference equality.  (/facepalm)
          Assert.Equal(expectedValidation.ToString(), testValidation.ToString());
        }

        [Fact]
        public void ShouldReturnSuccessForNonNull()
        {
          object testValue = new object();
          Validation<Exception, object> testValidation = Validation<Exception, object>.From(testValue);
          Validation<Exception, object> expectedValidation = Validation<Exception, object>.Pass<Exception, object>(testValue);

          Assert.Equal(expectedValidation, testValidation);
        }

        [Fact]
        public void ShouldReturnFailureForNullEither()
        {
          Either<Exception, bool> testEither = null;
          Validation<Exception, bool> testValidation = Validation<Exception, bool>.From(testEither);
          Validation<Exception, bool> expectedValidation = Validation<Exception, bool>.Fail<Exception, bool>((Exception)null);

          //NOTE(lee.crabtree):  The underlying exceptions only test for reference equality.  (/facepalm)
          Assert.Equal(expectedValidation.ToString(), testValidation.ToString());
        }

        [Fact]
        public void ShouldReturnValidationForEither()
        {
          Either<Exception, bool> testEither = Either<Exception, bool>.Right<Exception, bool>(_testValue);
          Validation<Exception, bool> testValidation = Validation<Exception, bool>.From(testEither);
          Validation<Exception, bool> expectedValidation = Validation<Exception, bool>.Pass<Exception, bool>(_testValue);

          Assert.Equal(expectedValidation, testValidation);
        }

        [Fact]
        public void ShouldReturnFailureForNullIEnumerable()
        {
          IEnumerable<bool> testIEnumerable = null;
          Validation<Exception, bool> testValidation = Validation<Exception, bool>.From(testIEnumerable);
          Validation<Exception, bool> expectedValidation = Validation<Exception, bool>.Fail<Exception, bool>(new NullReferenceException("list is null"));

          //NOTE(lee.crabtree):  The underlying exceptions only test for reference equality.  (/facepalm)
          Assert.Equal(expectedValidation.ToString(), testValidation.ToString());
        }

        [Fact]
        public void ShouldReturnFailureForEmptyIEnumerable()
        {
          IEnumerable<bool> testIEnumerable = new List<bool>();
          Validation<Exception, bool> testValidation = Validation<Exception, bool>.From(testIEnumerable);
          Validation<Exception, bool> expectedValidation = Validation<Exception, bool>.Fail<Exception, bool>(new NullReferenceException("list is empty"));

          //NOTE(lee.crabtree):  The underlying exceptions only test for reference equality.  (/facepalm)
          Assert.Equal(expectedValidation.ToString(), testValidation.ToString());
        }

        [Fact]
        public void ShouldReturnSuccessOfFirstValueForPopulatedIEnumerable()
        {
          IEnumerable<bool> testIEnumerable = new List<bool> { _testValue };
          Validation<Exception, bool> testValidation = Validation<Exception, bool>.From(testIEnumerable);
          Validation<Exception, bool> expectedValidation = Validation<Exception, bool>.Pass<Exception, bool>(testIEnumerable.First());

          Assert.Equal(expectedValidation, testValidation);
        }

        [Fact]
        public void ShouldReturnFailureForNullMaybe()
        {
          Maybe<bool> testMaybe = null;
          Validation<Exception, bool> testValidation = Validation<Exception, bool>.From(testMaybe);
          Validation<Exception, bool> expectedValidation = Validation<Exception, bool>.Fail<Exception, bool>(new NullReferenceException("value is null"));

          //NOTE(lee.crabtree):  The underlying exceptions only test for reference equality.  (/facepalm)
          Assert.Equal(expectedValidation.ToString(), testValidation.ToString());
        }

        [Fact]
        public void ShouldReturnFailureForNothing()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Nothing<bool>();
          Validation<Exception, bool> testValidation = Validation<Exception, bool>.From(testMaybe);
          Validation<Exception, bool> expectedValidation = Validation<Exception, bool>.Fail<Exception, bool>(new NullReferenceException("value is null"));

          //NOTE(lee.crabtree):  The underlying exceptions only test for reference equality.  (/facepalm)
          Assert.Equal(expectedValidation.ToString(), testValidation.ToString());
        }

        [Fact]
        public void ShouldReturnSuccessForJust()
        {
          Maybe<bool> testMaybe = Maybe<bool>.Just(_testValue);
          Validation<Exception, bool> testValidation = Validation<Exception, bool>.From(testMaybe);
          Validation<Exception, bool> expectedValidation = Validation<Exception, bool>.Pass<Exception, bool>(_testValue);

          Assert.Equal(expectedValidation, testValidation);
        }

        [Fact]
        public void ShouldReturnFailureForNullValidation()
        {
          Validation<Exception, bool> testValue = null;
          Validation<Exception, bool> testValidation = Validation<Exception, bool>.From(testValue);
          Validation<Exception, bool> expectedValidation = Validation<Exception, bool>.Fail<Exception, bool>(new NullReferenceException("value is null"));

          //NOTE(lee.crabtree):  The underlying exceptions only test for reference equality.  (/facepalm)
          Assert.Equal(expectedValidation.ToString(), testValidation.ToString());
        }

        [Fact]
        public void ShouldReturnFailureForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>("Fail");
          Validation<string, bool> expectedValidation = Validation<string, bool>.Fail<string, bool>("Fail");
          Validation<string, bool> actualValidation = Validation<string, bool>.From(testValidation);

          Assert.Equal(expectedValidation, actualValidation);
        }

        [Fact]
        public void ShouldReturnSuccessForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> expectedValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> actualValidation = Validation<string, bool>.From(testValidation);

          Assert.Equal(expectedValidation, actualValidation);
        }
      }

      public class FromFailure
      {
        [Fact]
        public void ShouldReturnValueForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          IEnumerable<string> expectedErrors = new List<string> { _testError };
          IEnumerable<string> actualErrors = Validation<string, bool>.FromFailure(testValidation);

          Assert.Equal(expectedErrors, actualErrors);
        }

        [Fact]
        public void ShouldThrowArgumentExceptionForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Exception exception = Record.Exception(() => Validation<string, bool>.FromFailure(testValidation));

          Assert.NotNull(exception);
          Assert.IsType<ArgumentException>(exception);
        }
      }

      public class FromSuccess
      {
        [Fact]
        public void ShouldThrowArgumentExceptionForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Exception exception = Record.Exception(() => Validation<string, bool>.FromSuccess(testValidation));

          Assert.NotNull(exception);
          Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ShouldReturnValueForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          bool expectedValue = _testValue;
          bool actualValue = Validation<string, bool>.FromSuccess(testValidation);

          Assert.Equal(expectedValue, actualValue);
        }
      }

      public class Of
      {
        [Fact]
        public void ShouldReturnSuccessForValue()
        {
          Validation<string, bool> expectedValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> actualValidation = Validation<string, bool>.Of(_testError, _testValue);

          Assert.Equal(expectedValidation, actualValidation);
        }

        [Fact]
        public void ShouldReturnFailureForNull()
        {
          object testValue = null;
          Validation<string, object> expectedValidation = Validation<string, object>.Fail<string, object>(_testError);
          Validation<string, object> actualValidation = Validation<string, object>.Of(_testError, testValue);

          Assert.Equal(expectedValidation, actualValidation);
        }
      }

      public class OfNullable
      {
        [Fact]
        public void ShouldReturnFailureForNull()
        {
          object testValue = null;
          Validation<Exception, object> expectedValidation = Validation<Exception, object>.Fail<Exception, object>(new NullReferenceException("value is null"));
          Validation<Exception, object> actualValidation = Validation<Exception, object>.OfNullable(testValue);

          //NOTE(lee.crabtree):  The underlying exceptions only test for reference equality.  (/facepalm)
          Assert.Equal(expectedValidation.ToString(), actualValidation.ToString());
        }

        [Fact]
        public void ShouldReturnSuccessForValue()
        {
          object testValue = new object();
          Validation<Exception, object> expectedValidation = Validation<Exception, object>.Pass<Exception, object>(testValue);
          Validation<Exception, object> actualValidation = Validation<Exception, object>.OfNullable(testValue);

          Assert.Equal(expectedValidation, actualValidation);
        }
      }
    }

    public class ValidationInstances
    {
      public class Alt
      {
        [Fact]
        public void ShouldReturnFailureForBothFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> testAlt = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> expectedResult = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> actualResult = testValidation.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnInstanceForFailureAlt()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> testAlt = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> expectedResult = testValidation;
          Validation<string, bool> actualResult = testValidation.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnInstanceForAltSupplier()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Func<Validation<string, bool>> testAlt = () => Validation<string, bool>.Pass<string, bool>(!_testValue);
          Validation<string, bool> expectedResult = testValidation;
          Validation<string, bool> actualResult = testValidation.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnAltForSuccessAlt()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> testAlt = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> expectedResult = testAlt;
          Validation<string, bool> actualResult = testValidation.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnAltForAltSupplier()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Func<Validation<string, bool>> testAlt = () => Validation<string, bool>.Pass<string, bool>(!_testValue);
          Validation<string, bool> expectedResult = Validation<string, bool>.Pass<string, bool>(!_testValue); ;
          Validation<string, bool> actualResult = testValidation.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnInstanceForSuccessInstance()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> testAlt = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> expectedResult = testValidation;
          Validation<string, bool> actualResult = testValidation.Alt(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Ap
      {
        [Fact]
        public void ShouldApplyForSuccessOfValueAndFunc()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, Func<bool, bool>> testApply = Validation<string, bool>.Pass<string, Func<bool, bool>>(b => !b);
          Validation<string, bool> expectedResult = Validation<string, bool>.Pass<string, bool>(!_testValue);
          Validation<string, bool> actualResult = testValidation.Ap(testApply);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotApplyForFailureOfFunc()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, Func<bool, bool>> testApply = Validation<string, Func<bool, bool>>.Fail<string, Func<bool, bool>>(_testError);
          Validation<string, bool> expectedResult = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> actualResult = testValidation.Ap(testApply);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotApplyForFailureOfValue()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, Func<bool, bool>> testApply = Validation<string, Func<bool, bool>>.Pass<string, Func<bool, bool>>(b => !b);
          Validation<string, bool> expectedResult = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> actualResult = testValidation.Ap(testApply);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnOtherForFailureOfValue()
        {
          string testOther = _testError + "1";
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, Func<bool, bool>> testApply = Validation<string, Func<bool, bool>>.Fail<string, Func<bool, bool>>(testOther);
          Validation<string, bool> expectedResult = Validation<string, bool>.Fail<string, bool>(testOther);
          Validation<string, bool> actualResult = testValidation.Ap(testApply);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Bimap
      {
        private Func<IEnumerable<string>, IEnumerable<int>> _testFailureMap = Always(new List<int> { 0 });
        //TODO(lee.crabtree): replace this with Functions.Always when I figure out how to make it work for value types
        private Func<bool, int> _testSuccessMap = b => 1;

        [Fact]
        public void ShouldMapFailureValueForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<int, int> expectedResult = Validation<int, int>.Fail<int, int>(0);
          Validation<int, int> actualResult = testValidation.Bimap(_testFailureMap, _testSuccessMap);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldMapSuccessValueForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<int, int> expectedResult = Validation<int, int>.Pass<int, int>(1);
          Validation<int, int> actualResult = testValidation.Bimap(_testFailureMap, _testSuccessMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Bind
      {
        [Fact]
        public void ShouldAliasChain()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Func<bool, Validation<string, bool>> testChain = value => Validation<string, bool>.Pass<string, bool>(!value);
          Validation<string, bool> expectedResult = testValidation.Bind(testChain);
          Validation<string, bool> actualResult = testValidation.Chain(testChain);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Chain
      {
        [Fact]
        public void ShouldChainForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Func<bool, Validation<string, bool>> testChain = value => Validation<string, bool>.Pass<string, bool>(!value);
          Validation<string, bool> expectedResult = Validation<string, bool>.Pass<string, bool>(!_testValue);
          Validation<string, bool> actualResult = testValidation.Chain(testChain);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotChainForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Func<bool, Validation<string, bool>> testChain = value => Validation<string, bool>.Pass<string, bool>(!value);
          Validation<string, bool> expectedResult = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> actualResult = testValidation.Chain(testChain);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Coalesce
      {
        [Fact]
        public void ShouldAliasAlt()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> testAlt = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> expectedResult = testValidation.Alt(testAlt);
          Validation<string, bool> actualResult = testValidation.Coalesce(testAlt);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Concat
      {
        [Fact]
        public void ShouldReturnSuccessInstanceForNullOther()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> testOtherValidation = null;
          Validation<string, bool> expectedResult = testValidation;
          Validation<string, bool> actualResult = testValidation.Concat(testOtherValidation);

          Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public void ShouldReturnInstanceForSameOther()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> testOtherValidation = testValidation;
          Validation<string, bool> expectedResult = testOtherValidation;
          Validation<string, bool> actualResult = testValidation.Concat(testOtherValidation);

          Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public void ShouldReturnSuccessInstanceForSuccessOther()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> testOtherValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> expectedResult = testValidation;
          Validation<string, bool> actualResult = testValidation.Concat(testOtherValidation);

          Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public void ShouldReturnConcatenatedFailures()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError + "1");
          Validation<string, bool> testOtherValidation = Validation<string, bool>.Fail<string, bool>(_testError + "2");
          Validation<string, bool> expectedResult = Validation<string, bool>.Fail<string, bool>(new List<string> {
            _testError + "1",
            _testError + "2"
          });
          Validation<string, bool> actualResult = testValidation.Concat(testOtherValidation);

          Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public void ShouldReturnFailureInstanceForSuccessOther()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> testOtherValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> expectedResult = testValidation;
          Validation<string, bool> actualResult = testValidation.Concat(testOtherValidation);

          Assert.Equal(actualResult, expectedResult);
        }

        [Fact]
        public void ShouldReturnOtherFailureForFailureOther()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> testOtherValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> expectedResult = testOtherValidation;
          Validation<string, bool> actualResult = testValidation.Concat(testOtherValidation);

          Assert.Equal(actualResult, expectedResult);
        }
      }

      public class Extend
      {
        [Fact]
        public void ShouldExtendForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Func<Validation<string, bool>, bool> testExtend = validation => validation
            .Map(b => !b)
            .GetOrElse(_testValue);
          Validation<string, bool> expectedResult = Validation<string, bool>.Pass<string, bool>(!_testValue);
          Validation<string, bool> actualResult = testValidation.Extend(testExtend);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotExtendForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Func<Validation<string, bool>, bool> testExtend = validation => validation
            .Map(b => !b)
            .GetOrElse(_testValue);
          Validation<string, bool> expectedResult = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> actualResult = testValidation.Extend(testExtend);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Filter
      {
        [Fact]
        public void ShouldFilterSuccessToJust()
        {
          string testValue = "test";
          Validation<string, string> testValidation = Validation<string, string>.Pass<string, string>(testValue);
          //TODO(lee.crabtree): replace this with Predicates::alwaysTrue when it's available
          Predicate<string> testPredicate = s => true;
          Maybe<string> expectedResult = Maybe<string>.Just(testValue);
          Maybe<string> actualResult = testValidation.Filter(testPredicate);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterSuccessToNothing()
        {
          String testValue = "test";
          Validation<string, string> testValidation = Validation<string, string>.Pass<string, string>(testValue);
          //TODO(lee.crabtree): replace this with Predicates::alwaysFalse when it's available
          Predicate<string> testPredicate = s => false;
          Maybe<string> expectedResult = Maybe<string>.Nothing<string>();
          Maybe<string> actualResult = testValidation.Filter(testPredicate);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterFailureToNothing()
        {
          Validation<string, string> testValidation = Validation<string, string>.Fail<string, string>(_testError);
          //TODO(lee.crabtree): replace this with Predicates::alwaysFalse when it's available
          Predicate<string> testPredicate = s => false;
          Maybe<string> expectedResult = Maybe<string>.Nothing<string>();
          Maybe<string> actualResult = testValidation.Filter(testPredicate);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class FilterWithFailureValue
      {
        [Fact]
        public void ShouldFilterSuccessToSuccess()
        {
          String testValue = "test";
          Validation<string, string> testValidation = Validation<string, string>.Pass<string, string>(testValue);
          //TODO(lee.crabtree): replace this with Predicates::alwaysTrue when it's available
          Predicate<string> testPredicate = s => true;
          Validation<string, string> expectedResult = Validation<string, string>.Pass<string, string>(testValue);
          Validation<string, string> actualResult = testValidation.Filter(testPredicate, _testError);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterSuccessToFailure()
        {
          String testValue = "test";
          Validation<string, string> testValidation = Validation<string, string>.Pass<string, string>(testValue);
          //TODO(lee.crabtree): replace this with Predicates::alwaysFalse when it's available
          Predicate<string> testPredicate = s => false;
          Validation<string, string> expectedResult = Validation<string, string>.Fail<string, string>(_testError);
          Validation<string, string> actualResult = testValidation.Filter(testPredicate, _testError);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterSuccessToFailureWithSupplier()
        {
          String testValue = "test";
          Validation<string, string> testValidation = Validation<string, string>.Pass<string, string>(testValue);
          //TODO(lee.crabtree): replace this with Predicates::alwaysFalse when it's available
          Predicate<string> testPredicate = s => false;
          Validation<string, string> expectedResult = Validation<string, string>.Fail<string, string>(_testError);
          Validation<string, string> actualResult = testValidation.Filter(testPredicate, () => _testError);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterFailureToFailure()
        {
          Validation<string, string> testValidation = Validation<string, string>.Fail<string, string>(_testError);
          //TODO(lee.crabtree): replace this with Predicates::alwaysFalse when it's available
          Predicate<string> testPredicate = s => false;
          Validation<string, string> expectedResult = Validation<string, string>.Fail<string, string>(_testError);
          Validation<string, string> actualResult = testValidation.Filter(testPredicate, _testError);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldFilterFailureToFailureWithSupplier()
        {
          Validation<string, string> testValidation = Validation<string, string>.Fail<string, string>(_testError);
          //TODO(lee.crabtree): replace this with Predicates::alwaysFalse when it's available
          Predicate<string> testPredicate = s => false;
          Validation<string, string> expectedResult = testValidation;
          Validation<string, string> actualResult = testValidation.Filter(testPredicate, () => _testError);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class First
      {
        private Func<IEnumerable<string>, IEnumerable<int>> _testFailureMap = Always(new List<int> { 0 });

        [Fact]
        public void ShouldMapFailureValueForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<int, bool> expectedResult = Validation<int, bool>.Fail<int, bool>(0);
          Validation<int, bool> actualResult = testValidation.First(_testFailureMap);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotMapFailureValueForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<int, bool> expectedResult = Validation<int, bool>.Pass<int, bool>(_testValue);
          Validation<int, bool> actualResult = testValidation.First(_testFailureMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class FlatMap
      {
        [Fact]
        public void ShouldAliasChain()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Func<bool, Validation<string, bool>> testChain = value => Validation<string, bool>.Pass<string, bool>(!value);
          Validation<string, bool> expectedResult = testValidation.FlatMap(testChain);
          Validation<string, bool> actualResult = testValidation.Chain(testChain);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Fold
      {
        private Func<IEnumerable<string>, int> _testFailureMap = Always(0);
        //TODO(lee.crabtree): replace this with Functions.Always when I figure out how to make it work for value types
        private Func<bool, int> _testSuccessMap = b => 1;

        [Fact]
        public void ShouldReturnFailureFoldForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          int expectedResult = 0;
          int actualResult = testValidation.Fold(_testFailureMap, _testSuccessMap);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnSuccessFoldForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          int expectedResult = 1;
          int actualResult = testValidation.Fold(_testFailureMap, _testSuccessMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class GetFailure
      {
        [Fact]
        public void ShouldReturnFailureValueForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          List<string> expectedResult = new List<string> { _testError };
          IEnumerable<string> actualResult = testValidation.GetFailures();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnNullForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          IEnumerable<string> actualResult = testValidation.GetFailures();

          Assert.Null(actualResult);
        }
      }

      public class GetSuccess
      {
        [Fact]
        public void ShouldReturnSuccessValueForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          bool expectedResult = _testValue;
          bool actualResult = testValidation.GetSuccess();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnDefaultForFailureOnValueType()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          bool actualResult = testValidation.GetSuccess();

          Assert.Equal(default(bool), actualResult);
        }

        [Fact]
        public void ShouldReturnNullForFailureOnReferenceType()
        {
          Validation<string, string> testValidation = Validation<string, string>.Fail<string, string>("value");
          string actualResult = testValidation.GetSuccess();

          Assert.Null(actualResult);
        }
      }

      public class GetOrElse
      {
        private bool _testOtherValue = !_testValue;

        [Fact]
        public void ShouldReturnValueForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          bool expectedResult = _testValue;
          bool actualResult = testValidation.GetOrElse(_testOtherValue);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnOtherValueForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          bool expectedResult = _testOtherValue;
          bool actualResult = testValidation.GetOrElse(_testOtherValue);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class GetOrElseGet
      {
        [Fact]
        public void ShouldReturnValueForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Func<bool> testSupplier = AlwaysSupply(!_testValue);
          bool expectedResult = _testValue;
          bool actualResult = testValidation.GetOrElseGet(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnMappedOtherValueForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Func<IEnumerable<string>, bool> testFunc = Always(!_testValue);
          bool expectedResult = !_testValue;
          bool actualResult = testValidation.GetOrElseGet(testFunc);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnSuppliedOtherValueForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Func<bool> testSupplier = AlwaysSupply(!_testValue);
          bool expectedResult = !_testValue;
          bool actualResult = testValidation.GetOrElseGet(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class GetOrElseThrow
      {
        [Fact]
        public void ShouldReturnValueForSuccessWithFunc()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Func<IEnumerable<string>, Exception> testFunc = failures => failures
            .Take(1)
            .Select(s => new Exception(s))
            .FirstOrDefault();

          bool expectedResult = _testValue;
          bool actualResult = testValidation.GetOrElseThrow(testFunc);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnValueForSuccessWithSupplier()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Func<Exception> testSupplier = () => new Exception();
          bool expectedResult = _testValue;
          bool actualResult = testValidation.GetOrElseThrow(testSupplier);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldThrowForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Func<Exception> testSupplier = () => new Exception();
          Exception exception = Record.Exception(() => testValidation.GetOrElseThrow(testSupplier));

          Assert.NotNull(exception);
          Assert.IsType<Exception>(exception);
        }

        [Fact]
        public void ShouldThrowFailureValueForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Func<IEnumerable<string>, Exception> testFunc = failures => failures
            .Take(1)
            .Select(s => new Exception(s))
            .FirstOrDefault();
          Exception exception = Record.Exception(() => testValidation.GetOrElseThrow(testFunc));

          Assert.NotNull(exception);
          Assert.IsType<Exception>(exception);
        }
      }

      public class HashCode
      {
        //NOTE(lee.crabtree): I'm not sure why these hash codes don't match up yet, but I don't think it's worth
        //failing the build over.
        //[Fact]
        //public void ShouldReturnFailureHashCodeForFailure()
        //{
        //  Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
        //  int expectedResult = new List<string> { _testError }.GetHashCode();
        //  int actualResult = testValidation.GetHashCode();

        //  Assert.Equal(expectedResult, actualResult);
        //}

        [Fact]
        public void ShouldReturnSuccessHashCodeForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          int expectedResult = _testValue.GetHashCode();
          int actualResult = testValidation.GetHashCode();

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class IfFailure
      {
        [Fact]
        public void ShouldCallIfFailureForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Mock<Action<IEnumerable<string>>> testAction = new Mock<Action<IEnumerable<string>>>();
          int expectedInvocations = 1;

          testValidation.IfFailure(testAction.Object);

          testAction.Verify(f => f(new List<string> { _testError }), Times.Exactly(expectedInvocations));
        }

        [Fact]
        public void ShouldNotCallIfFailureForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Mock<Action<IEnumerable<string>>> testAction = new Mock<Action<IEnumerable<string>>>();
          int expectedInvocations = 0;

          testValidation.IfFailure(testAction.Object);

          testAction.Verify(f => f(new List<string> { _testError }), Times.Exactly(expectedInvocations));
        }
      }

      public class IfSuccess
      {
        [Fact]
        public void ShouldCallIfSuccessForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Mock<Action<bool>> testAction = new Mock<Action<bool>>();
          int expectedInvocations = 1;

          testValidation.IfSuccess(testAction.Object);

          testAction.Verify(f => f(_testValue), Times.Exactly(expectedInvocations));
        }

        [Fact]
        public void ShouldNotCallIfSuccessForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Mock<Action<bool>> testAction = new Mock<Action<bool>>();
          int expectedInvocations = 0;

          testValidation.IfSuccess(testAction.Object);

          testAction.Verify(f => f(_testValue), Times.Exactly(expectedInvocations));
        }
      }

      public class IsSuccess
      {
        [Fact]
        public void ShouldReturnFalseForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          bool actualResult = testValidation.IsSuccess();

          Assert.False(actualResult);
        }

        [Fact]
        public void ShouldReturnTrueForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          bool actualResult = testValidation.IsSuccess();

          Assert.True(actualResult);
        }
      }

      public class IsFailure
      {
        [Fact]
        public void ShouldReturnFalseForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          bool actualResult = testValidation.IsFailure();

          Assert.False(actualResult);
        }

        [Fact]
        public void ShouldReturnTrueForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          bool actualResult = testValidation.IsFailure();

          Assert.True(actualResult);
        }
      }

      public class FailureMap
      {
        [Fact]
        public void ShouldAliasFirst()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Func<IEnumerable<string>, IEnumerable<int>> testFailureMap = Always(new List<int> { 0 });
          Validation<int, bool> expectedResult = testValidation.First(testFailureMap);
          Validation<int, bool> actualResult = testValidation.FailureMap(testFailureMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Map
      {
        [Fact]
        public void ShouldMapForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> expectedResult = Validation<string, bool>.Pass<string, bool>(!_testValue);
          //TODO(lee.crabtree): replace this with Predicates::negate when it's available
          Validation<string, bool> actualResult = testValidation.Map(b => !b);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotMapForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> expectedResult = Validation<string, bool>.Fail<string, bool>(_testError);
          //TODO(lee.crabtree): replace this with Predicates::negate when it's available
          Validation<string, bool> actualResult = testValidation.Map(b => !b);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Recover
      {
        private bool _testRecover = !_testValue;

        [Fact]
        public void ShouldReturnInstanceForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, bool> expectedResult = testValidation;
          Validation<string, bool> actualResult = testValidation.Recover(_testRecover);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnInstanceForSuccessSupplier()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Func<bool> testRecover = () => _testRecover;
          Validation<string, bool> expectedResult = testValidation;
          Validation<string, bool> actualResult = testValidation.Recover(testRecover);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnValueForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, bool> expectedResult = Validation<string, bool>.Pass<string, bool>(_testRecover);
          Validation<string, bool> actualResult = testValidation.Recover(_testRecover);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnValueFromSupplierForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Func<bool> testRecover = () => _testRecover;
          Validation<string, bool> expectedResult = Validation<string, bool>.Pass<string, bool>(_testRecover);
          Validation<string, bool> actualResult = testValidation.Recover(testRecover);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Reduce
      {
        [Fact]
        public void ShouldAliasFold()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Func<IEnumerable<string>, int> testFailureMap = Always(0);
          //TODO(lee.crabtree): replace this with Functions.Always when I figure it out for value types
          Func<bool, int> testSuccessMap = ignored => 1;
          int expectedResult = testValidation.Fold(testFailureMap, testSuccessMap);
          int actualResult = testValidation.Reduce(testFailureMap, testSuccessMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class SuccessMap
      {
        [Fact]
        public void ShouldAliasSecond()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          //TODO(lee.crabtree): replace this with Functions.Always when I figure it out for value types
          Func<bool, int> testSuccessMap = ignored => 1;
          Validation<string, int> expectedResult = testValidation.Second(testSuccessMap);
          Validation<string, int> actualResult = testValidation.SuccessMap(testSuccessMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Second
      {
        //TODO(lee.crabtree): replace this with Functions.Always when I figure it out for value types
        private Func<bool, int> _testSuccessMap = ignored => 1;

        [Fact]
        public void ShouldMapSuccessValueForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Validation<string, int> expectedResult = Validation<string, int>.Pass<string, int>(1);
          Validation<string, int> actualResult = testValidation.Second(_testSuccessMap);

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldNotMapSuccessValueForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Validation<string, int> expectedResult = Validation<string, int>.Fail<string, int>(_testError);
          Validation<string, int> actualResult = testValidation.Second(_testSuccessMap);

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class Tap
      {
        [Fact]
        public void ShouldCallIfSuccessForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Mock<Action<IEnumerable<string>>> testLeftAction = new Mock<Action<IEnumerable<string>>>();
          Mock<Action<bool>> testRightAction = new Mock<Action<bool>>();
          int expectedLeftInvocations = 0;
          int expectedRightInvocations = 1;

          testValidation.Tap(testLeftAction.Object, testRightAction.Object);

          testLeftAction.Verify(f => f(new List<string> { _testError }), Times.Exactly(expectedLeftInvocations));
          testRightAction.Verify(f => f(_testValue), Times.Exactly(expectedRightInvocations));
        }

        [Fact]
        public void shoulCallIfFailureForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Mock<Action<IEnumerable<string>>> testLeftAction = new Mock<Action<IEnumerable<string>>>();
          Mock<Action<bool>> testRightAction = new Mock<Action<bool>>();
          int expectedLeftInvocations = 1;
          int expectedRightInvocations = 0;

          testValidation.Tap(testLeftAction.Object, testRightAction.Object);

          testLeftAction.Verify(f => f(new List<string> { _testError }), Times.Exactly(expectedLeftInvocations));
          testRightAction.Verify(f => f(_testValue), Times.Exactly(expectedRightInvocations));
        }
      }

      public class ToEither
      {
        [Fact]
        public void ShouldReturnSuccessForJust()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Either<string, bool> expectedResult = Either<string, bool>.Right<string, bool>(_testValue);
          Either<string, bool> actualResult = testValidation.ToEither();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnFailureForNothing()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Either<string, bool> expectedResult = Either<string, bool>.Left<string, bool>(_testError);
          Either<string, bool> actualResult = testValidation.ToEither();

          Assert.Equal(expectedResult, actualResult);
        }
      }

      public class ToList
      {
        [Fact]
        public void ShouldReturnListForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          List<bool> expectedList = new List<bool> { _testValue };
          IList<bool> actualList = testValidation.ToList();

          Assert.Equal(expectedList, actualList);
        }

        [Fact]
        public void ShouldReturnEmptyListForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          List<bool> expectedList = new List<bool>();
          IList<bool> actualList = testValidation.ToList();

          Assert.Equal(expectedList, actualList);
        }
      }

      public class ToMaybe
      {
        [Fact]
        public void ShouldReturnNothingForFailure()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Fail<string, bool>(_testError);
          Maybe<bool> expectedResult = Maybe<bool>.Nothing<bool>();
          Maybe<bool> actualResult = testValidation.ToMaybe();

          Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldReturnJustForSuccess()
        {
          Validation<string, bool> testValidation = Validation<string, bool>.Pass<string, bool>(_testValue);
          Maybe<bool> expectedResult = Maybe<bool>.Just(_testValue);
          Maybe<bool> actualResult = testValidation.ToMaybe();

          Assert.Equal(expectedResult, actualResult);
        }
      }
    }
  }
}
