using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Highbar
{
	[TestClass]
	public class EitherTests
	{
		private static readonly Func<Either<int>,Either<int>> _apply = e =>
			e.Map(x => x + 1).Map(x => x + 1);

		[TestMethod]
		public void ErrorsShouldNotHaveValues()
		{
			_apply(Either.Error<int>(new Exception("whoops")))
				.OnValue(v => Assert.Fail("Either should not have a value: {0}", v))
				.OnError(e => e.Message.Should().Be("whoops"));
		}

		[TestMethod]
		public void ValuesShouldNotHaveErrors()
		{
			_apply(Either.Value(0))
				.OnValue(v => v.Should().Be(2))
				.OnError(e => Assert.Fail("Either should not have an error: {0}", e));
		}
	}
}
