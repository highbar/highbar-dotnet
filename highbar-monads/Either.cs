using System;

namespace Highbar
{
	public static class Either
	{
		public static Either<T> Error<T>(Exception error)
		{
			return new Either<T>(error);
		}

		public static Either<T> Value<T>(T value)
		{
			return new Either<T>(value: value);
		}
	}

	public class Either<T>
	{
		private readonly bool _isError;
		private readonly Exception _error;
		private readonly T _value;

		public Either(Exception error = null, T value = default(T))
		{
			_isError = error != null;
			_error = error;
			_value = value;
		}

		public Either<T2> Map<T2>(Func<T,T2> handler)
		{
			if (_isError) return Either.Error<T2>(_error);

			try
			{
				return Either.Value(handler(_value));
			}
			catch (Exception error)
			{
				return Either.Error<T2>(error);
			}
		}

		public Either<T> OnError(Action<Exception> handler)
		{
			if (_isError) handler(_error);

			return this;
		}

		public Either<T> OnValue(Action<T> handler)
		{
			if (!_isError) handler(_value);

			return this;
		}
	}
}
