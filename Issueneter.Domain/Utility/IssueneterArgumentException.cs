using System.Runtime.CompilerServices;

namespace Issueneter.Domain.Utility;

public class IssueneterArgumentException
{
    public static ArgumentOutOfRangeException For<T>(T argument, [CallerArgumentExpression("argument")] string? argumentName = null) where T : notnull
    {
        return new ArgumentOutOfRangeException(argumentName, $"Unexpected argument type {argument.GetType()}");
    }
}