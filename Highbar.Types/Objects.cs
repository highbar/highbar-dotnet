using System;

namespace Highbar.Types
{
  public static class Objects
  {
    public static bool IsNotNull<T>(T value)
    {
      return !IsNull(value);
    }

    public static bool IsNull<T>(T value)
    {
      return value == null;
    }

    public static T RequireNonNull<T>(T obj, string errorString)
    {
      if(obj == null)
      {
        throw new ArgumentNullException(errorString);
      }

      return obj;
    }

    public static T RequireNonNull<T>(T obj)
    {
      if(obj == null)
      {
        throw new ArgumentNullException(nameof(obj));
      }

      return obj;
    }
  }
}
