namespace server.Exceptions;

public class CategoryAlreadyExistException : Exception
{
    public CategoryAlreadyExistException(string? message) : base(message)
    {
    }
}
