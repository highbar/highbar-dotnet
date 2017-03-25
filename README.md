# highbar-dotnet

Highbar for .NET Core

```bash
# Either
dotnet add package highbar-monads

# Promise, Try
dotnet add package highbar-promises
```

```csharp
using System;
using Highbar;

namespace Pandimensional.HyperIntelligent
{
  static class Program
  {
    public static void Main(string[] args)
    {
      Try.Get(DeepThought.CalculateAnswer)
        .Tap(Console.WriteLine)
        .Then(Earth.GetQuestion)
        .Tap(Console.WriteLine)
        .Catch(Console.WriteLine);
    }
  }
}
```

```bash
42
VogonAttackException: CONNECTION ABORTED
```
