namespace server.Exceptions;

public class InvalidProductException : ArgumentException
{
    public InvalidProductException(string? message) : base(message)
    {
    }
}
