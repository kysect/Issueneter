namespace Issueneter.Filters;

public class IssueneterValidationException : Exception
{
    public IssueneterValidationException(string message) : base(message)
    {
    }
}