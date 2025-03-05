namespace server.Exceptions;

public class NoProductsAvailableException : Exception
{
    public NoProductsAvailableException(string? message) : base(message)
    {
    }
}
